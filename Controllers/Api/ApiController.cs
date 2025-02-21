using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Models.Requests;
using SIGES.Repositorios;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace SIGES.Controllers.Api
{
    [Route("api/v1")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ICobranca _cobrancaRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IVenda _vendaRepository;
        private readonly IItensVenda _itensVendasRepository;
        private readonly IOs _osRepository;
        private readonly IClienteRepositorio _clienteRepository;
        private readonly IRepository<LancamentoModel> _lancamentoRepository;

        public ApiController(ICobranca cobrancaRepository, IPaymentGateway paymentGateway, IVenda vendaRepository,
            IOs osRepository, IItensVenda itensVendasRepository, IClienteRepositorio clienteRepository, IRepository<LancamentoModel> lancamentoRepository)
        {
            _cobrancaRepository = cobrancaRepository;
            _paymentGateway = paymentGateway;
            _vendaRepository = vendaRepository;
            _osRepository = osRepository;
            _itensVendasRepository = itensVendasRepository;
            _clienteRepository = clienteRepository;
            _lancamentoRepository = lancamentoRepository;
        }

        [HttpPost("adicionar")]
        [Permissao("aCobranca", "Você não tem permissão para adicionar cobrança!")]
        public async Task<IActionResult> CriarCobranca([FromBody] CobrancaRequest request)
        {
            try
            {
                var cobrancas = await _cobrancaRepository.Listar();
                var cobrancasExistentes = cobrancas.Where(x => (request.Tipo == "venda" ? x.VendasId : x.OsId) == request.Id).ToList();

                if (cobrancasExistentes.Any())
                {
                    return Ok(new { Status = "Erro!", Message = "Já existe cobrança!", Acao = "error" });
                }

                var (totalValor, cliente, errosCadastro, tipoDesconto, desconto) = await BuscarDadosVendaOuOS(request.Id, request.Tipo);
                // Verifica se há erros no cadastro do cliente
                if (errosCadastro.Any())
                {
                    string mensagemErro = "Por favor, preencha os seguintes dados do(a) cliente:\n\n" + string.Join("\n", errosCadastro);
                    return Ok(new { Status = "Erro!", Message = mensagemErro, Acao = "error" });
                }

                // Preparar os dados do cliente para pagamento
                string documento = Regex.Replace(cliente.Documento, "[^0-9]", "");
                string name = cliente.Nome;
                string[] words = name.Split(' ');

                // Criar o objeto de requisição de pagamento
                var paymentRequest = new PaymentRequest
                {
                    Id = request.Id,
                    Tipo = request.Tipo,
                    TransactionAmount = totalValor,
                    Description = request.Tipo == "venda" ? $"Venda #{request.Id}" : $"Ordem de Serviço #{request.Id}",
                    PaymentMethodId = "bolbradesco",
                    Identification = documento,
                    ZipCode = Regex.Replace(cliente.Cep, "[^0-9]", ""),
                    StreetName = cliente.Rua,
                    StreetNumber = cliente.NumeroRua,
                    Neighborhood = cliente.Bairro,
                    City = cliente.Cidade,
                    FederalUnit = cliente.Estado,
                    Email = cliente.Email ?? "",
                    ClienteId = cliente.Id,
                    FirstName = words.Length > 0 ? words[0] : "",
                    LastName = words.Length > 1 ? words[words.Length - 1] : "",
                };

                var cobranca = await _paymentGateway.CriarCobranca(paymentRequest);
                return Ok(cobranca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        private async Task<(decimal TotalValor, ClienteModel Cliente, List<string> ErrosCadastro, string TipoDesconto, decimal Desconto)>
            BuscarDadosVendaOuOS(int id, string tipo)
        {
            // Buscar os dados da entidade (Venda ou OS)
            object entidade = tipo == "venda"
                ? await _vendaRepository.ObterPorId(id)
                : await _osRepository.ObterPorId(id);

            if (entidade == null)
            {
                throw new Exception("OS ou venda não encontrada!");
            }

            // Obter produtos e serviços
            var produtos = (tipo == "venda"
                ? _itensVendasRepository.ObterPorVendaId(id)
                : _itensVendasRepository.ObterPorVendaId(id)
            ).ToList();

            var servicos = (tipo == "venda"
                ? new List<ServicosOs>()
                : _osRepository.GetServicos(id)
            )?.ToList() ?? new List<ServicosOs>();

            if (!produtos.Any())
            {
                throw new Exception("Não existem produtos incluídos para a venda ou OS!");
            }

            decimal desconto = 0;
            string tipoDesconto = "real";
            int clienteId = 0;
            if (tipo == "venda")
            {
                var venda = entidade as VendaModel;
                desconto = venda?.Desconto ?? 0;
                tipoDesconto = venda?.TipoDesconto ?? "real";
                clienteId = venda?.ClienteId ?? 0;
            }
            else
            {
                var os = entidade as OsModel;
                clienteId = os?.ClientesId ?? 0;
                desconto = os?.Desconto ?? 0;
                tipoDesconto = os?.TipoDesconto ?? "real";
            }

            decimal qtd = produtos.Sum(x => x.Quantidade) ?? 0;
            decimal valor = produtos.Sum(x => x.Preco) ?? 0;

            decimal totalProdutos = valor * qtd;
            decimal totalServicos = servicos.Sum(x => x.Preco * Convert.ToDecimal(x.Quantidade) ?? 0);

            if (totalProdutos <= 0)
            {
                throw new Exception("OS ou venda com valor negativo ou zero!");
            }

            decimal totalValor = ValorTotal(totalProdutos, totalServicos, desconto, tipoDesconto);

            // Buscar cliente
            var cliente = await _clienteRepository.ObterPorId(clienteId);
            if (cliente == null)
            {
                throw new Exception("OS ou venda sem cliente vinculado!");
            }

            // Verificar erros no cadastro do cliente
            List<string> errosCadastro = ErrosCadastro(cliente);

            return (totalValor, cliente, errosCadastro, tipoDesconto, desconto);
        }

        [HttpPost("cancelar/{id}")]
        public async Task<IActionResult> CancelarCobranca(int id)
        {
            try
            {
                bool cancelado = await _paymentGateway.CancelarCobranca(id);
                if (cancelado)
                    return Ok(new { message = "Cobrança cancelada!" });

                return BadRequest(new { message = "Cobrança não pode ser cancelada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        public decimal ValorTotal(decimal produtosValor, decimal servicosValor, decimal desconto, string tipoDesconto)
        {
            decimal defDesconto = 0;
            
            if (tipoDesconto == "porcento")
            {
                defDesconto = desconto * (produtosValor + servicosValor) / 100;
            }
            else if (tipoDesconto == "real")
            {
                defDesconto = desconto;
            }
            else
            {
                defDesconto = 0;
            }

            return (produtosValor + servicosValor) - desconto;
        }

        /// <summary>
        /// Método que retorna se tiver algum dado do cliente vazio
        /// </summary>
        public List<string> ErrosCadastro(ClienteModel cliente)
        {
            List<string> erros = new List<string>();

            if (cliente == null)
            {
                erros.Add("Cliente não pode ser nulo.");
                return erros;
            }

            // Lista de campos obrigatórios
            List<string> camposObrigatorios = new List<string>
            {
                "Rua", "Numero", "Bairro", "Cep", "Cidade", "Estado", "Documento", "Telefone", "Nome", "Email"
            };

            // Percorre as propriedades do objeto cliente e verifica se algum valor é inválido
            foreach (var propriedade in cliente.GetType().GetProperties())
            {
                if (camposObrigatorios.Contains(propriedade.Name))
                {
                    var valor = propriedade.GetValue(cliente);

                    if (valor == null || string.IsNullOrWhiteSpace(valor.ToString()) || (valor.ToString().Length < 2 && !int.TryParse(valor.ToString(), out _)))
                    {
                        erros.Add($"- {propriedade.Name}");
                    }
                }
            }

            return erros;
        }

        [HttpPost("faturar")]
        [Permissao("eVenda", "Você não tem permissão para adicionar cobrança!")]

        public async Task<IActionResult> Faturar([FromBody] FaturaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new { Status = "Erro!", Message = "Erro ao gerar cobrança!", Acao = "error" });
                }

                if (request.Grupo == "venda")
                {
                    var venda = await _vendaRepository.ObterPorId(request.VendaId ?? 0);
                    if (venda == null)
                    {
                        return Ok(new { Status = "Erro!", mensagem = "Venda não encontrada!", Acao = "error" });
                    }

                    if (request.ValorDesconto < 0)
                        request.ValorDesconto = 0;

                    if (request.ValorTotal <= 0)
                    {
                        return Ok(new { Status = "Erro!", mensagem = "Não é possível faturar venda sem produtos!", Acao = "error" });
                    }

                    var lancamento = new LancamentoModel
                    {
                        VendasId = request.VendaId,
                        Descricao = request.Descricao,
                        Valor = request.ValorTotal,
                        Desconto = request.ValorTotal - request.ValorDesconto,
                        ValorDesconto = request.ValorDesconto,
                        ClientesId = request.ClienteId,
                        DataVencimento = request.DataVencimento,
                        DataPagamento = request.Recebido == 1 ? request.DataRecebimento : null,
                        Baixado = request.Recebido,
                        FormaPgto = request.Recebido == 1 ? request.FormaPgto : null,
                        Tipo = request.Tipo,
                        UsuariosId = int.Parse(User.FindFirst("UserId")?.Value ?? "0")
                    };

                    await _lancamentoRepository.Adicionar(lancamento);

                    venda.Faturado = 1;
                    venda.ValorTotal = request.ValorTotal;
                    venda.Desconto = request.ValorTotal - request.ValorDesconto;
                    venda.ValorDesconto = request.ValorDesconto;
                    venda.LancamentoId = lancamento.Id;
                    venda.Status = "Faturado";

                    await _vendaRepository.Atualizar(venda);

                    
                    return Ok(new { Status = "Sucesso!", mensagem = "Venda faturada com sucesso!!", Acao = "success" });
                }
                else
                {

                }

                return Ok(new { Status = "Erro!", mensagem = "Erro ao faturar!", Acao = "error" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Erro!", Message = ex.Message, Acao = "error" });
            }
        }
    }
}
