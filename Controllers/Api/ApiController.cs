using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Models.Requests;
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

        public ApiController(ICobranca cobrancaRepository, IPaymentGateway paymentGateway, IVenda vendaRepository,
            IOs osRepository, IItensVenda itensVendasRepository, IClienteRepositorio clienteRepository)
        {
            _cobrancaRepository = cobrancaRepository;
            _paymentGateway = paymentGateway;
            _vendaRepository = vendaRepository;
            _osRepository = osRepository;
            _itensVendasRepository = itensVendasRepository;
            _clienteRepository = clienteRepository;
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

                var produtos = request.Tipo == "venda" ? _vendaRepository.GetProdutos(request.Id) : _vendaRepository.GetProdutos(request.Id);

                var paymentRequest = new PaymentRequest();
                if (request.Tipo == "venda")
                {
                    var venda = await _vendaRepository.ObterPorId(request.Id);
                    var produtos = _itensVendasRepository.ObterPorVendaId(request.Id);

                    if (!produtos.Any())
                    {
                        return Ok(new { Status = "Erro!", Message = "Não existe produtos incluídos para a venda!", Acao = "error" });
                    }

                    var qtd = produtos.Sum(x => x.Quantidade) ?? 0;
                    var valor = produtos.Sum(x => x.Preco) ?? 0;
                    var desconto = venda.Desconto ?? 0;
                    var tipoDesconto = venda.TipoDesconto ?? "";

                    var totalProdutos = valor * qtd;
                    var totalServicos = 0;

                    if (totalProdutos <= 0)
                    {
                        return Ok(new { Status = "Erro!", Message = "OS ou venda com valor negativo ou zero!", Acao = "error" });
                    }

                    var totalValor = ValorTotal(totalProdutos, totalServicos, desconto, tipoDesconto);

                    var cliente = await _clienteRepository.ObterPorId(venda.Cliente.Id);
                    if (cliente == null)
                    {
                        return Ok(new { Status = "Erro!", Message = "OS ou venda sem cliente vinculado!", Acao = "error" });
                    }

                    List<string> errosCadastro = ErrosCadastro(cliente);
                    if (errosCadastro.Any())
                    {
                        string mensagemErro = "Por favor, preencha os seguintes dados do(a) cliente:\n\n" + string.Join("\n", errosCadastro);
                        return Ok(new { Status = "Erro!", Message = mensagemErro, Acao = "error" });
                    }

                    string documento = Regex.Replace(venda.Cliente.Documento, "[^0-9]", "");
                    string name = venda.Cliente.Nome;
                    string[] words = name.Split(' ');

                    paymentRequest = new PaymentRequest
                    {
                        Id = request.Id,
                        Tipo = request.Tipo,
                        TransactionAmount = totalValor,
                        Description = $"Venda #{request.Id}",
                        PaymentMethodId = "bolbradesco",
                        Identification = documento,
                        ZipCode = Regex.Replace(venda.Cliente.Cep, "[^0-9]", ""),
                        StreetName = venda.Cliente.Rua,
                        StreetNumber = venda.Cliente.NumeroRua,
                        Neighborhood = venda.Cliente.Bairro,
                        City = venda.Cliente.Cidade,
                        FederalUnit = venda.Cliente.Estado,
                        Email = venda.Cliente.Email ?? "",
                        ClienteId = venda.Cliente.Id,
                        FirstName = words[1],
                        LastName = words[words.Length - 1],
                    };
                }
                else
                {
                    var os = await _osRepository.ObterPorId(request.Id);

                    string documento = Regex.Replace(os.Cliente.Documento, "[^0-9]", "");
                    string name = os.Cliente.Nome;
                    string[] words = name.Split(' ');

                    paymentRequest = new PaymentRequest
                    {
                        Id = request.Id,
                        Tipo = request.Tipo,
                        //TransactionAmount = os.,
                        Description = $"Ordem de Serviço #{request.Id}",
                        PaymentMethodId = "bolbradesco",
                        Identification = documento,
                        ZipCode = Regex.Replace(os.Cliente.Cep, "[^0-9]", ""),
                        StreetName = os.Cliente.Rua,
                        StreetNumber = os.Cliente.NumeroRua,
                        Neighborhood = os.Cliente.Bairro,
                        City = os.Cliente.Cidade,
                        FederalUnit = os.Cliente.Estado,
                        Email = os.Cliente.Email ?? "",
                        ClienteId = os.Cliente.Id,
                        FirstName = words[0],
                        LastName = words[words.Length - 1],
                    };
                }

                var cobranca = await _paymentGateway.CriarCobranca(paymentRequest);
                return Ok(cobranca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("confirmar-pagamento/{id}")]
        public async Task<IActionResult> ConfirmarPagamento(int id)
        {
            try
            {
                bool confirmado = await _paymentGateway.ConfirmarPagamento(id);
                if (confirmado)
                    return Ok(new { message = "Pagamento confirmado!" });

                return BadRequest(new { message = "Pagamento não confirmado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
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
    }
}
