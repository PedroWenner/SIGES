using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Route("lancamentos")]
    [Authorize]
    public class LancamentoController : Controller
    {
        private readonly IRepository<LancamentoModel> _lancamentoRepository;
        private readonly ILancamento _lancamentoRepositorio;

        public LancamentoController(IRepository<LancamentoModel> repositorio, ILancamento lancamentoRepositorio)
        {
            _lancamentoRepository = repositorio;
            _lancamentoRepositorio = lancamentoRepositorio;
        }

        [Permissao("vLancamento", "Você não tem permissão para visualizar lancamentos")]
        public async Task<IActionResult> Index()
        {
            var lancamentos = await _lancamentoRepository.Listar();
            return View("~/views/Lancamentos/lLancamento.cshtml", new LancamentoViewModel { Lancamentos = lancamentos.ToList() });
        }

        [Permissao("vLancamento", "Você não tem permissão para visualizar lancamentos")]
        [HttpPost("filtrar")]
        public async Task<IActionResult> Filtrar([FromBody] FiltroModel filtro)
        {
            try
            {
                var dadosFiltrados = await _lancamentoRepositorio.GetVendasOs();
                if (dadosFiltrados.Any())
                {
                    dadosFiltrados = dadosFiltrados
                    .Where(x =>
                        x.DataVencimento >= filtro.VencimentoDe &&
                        x.DataVencimento <= filtro.VencimentoAte &&
                        (filtro.Status == 99 || x.Baixado == filtro.Status) &&
                        (filtro.Tipo == "todos" || x.Tipo == filtro.Tipo) &&
                        (filtro.ClienteId == null || x.ClientesId == filtro.ClienteId)
                    )
                    .ToList();

                    decimal totalReceitasPago = dadosFiltrados.Where(x => x.Baixado == 1 && x.Tipo == "receita").Sum(x => x.ValorDesconto);
                    decimal totalDespesasPago = dadosFiltrados.Where(x => x.Baixado == 1 && x.Tipo == "despesa").Sum(x => x.ValorDesconto);
                    decimal totalLiquidoPago = totalReceitasPago - totalDespesasPago;

                    decimal totalReceitasPendentes = dadosFiltrados.Where(x => x.Baixado == 0 && x.Tipo == "receita").Sum(x => x.ValorDesconto);
                    decimal totalDespesasPendentes = dadosFiltrados.Where(x => x.Baixado == 0 && x.Tipo == "despesa").Sum(x => x.ValorDesconto);
                    decimal totalLiquidoPendentes = totalReceitasPendentes - totalDespesasPendentes;

                    decimal totalReceitas = totalReceitasPago + totalReceitasPendentes;
                    decimal totalDespesas = totalDespesasPago + totalDespesasPendentes;
                    decimal totalLiquido = totalReceitas + totalDespesas;

                    decimal totalDescontosPago = dadosFiltrados.Where(x => x.Baixado == 1 && x.Desconto > 0).Sum(x => x.Desconto);
                    decimal totalDescontosPendentes = dadosFiltrados.Where(x => x.Baixado == 0 && x.Desconto > 0).Sum(x => x.Desconto);
                    decimal totalLiquidoDescontos = totalDescontosPago + totalDescontosPendentes;

                    decimal totalReceitaDescontos = dadosFiltrados.Where(x => x.Tipo == "receita" && x.Desconto == 0).Sum(x => x.Valor);
                    decimal totalDespesasDescontos = dadosFiltrados.Where(x => x.Tipo == "despesa" && x.Desconto == 0).Sum(x => x.Valor);

                    return Json(new {
                        success = true,
                        totalReceitasPago = totalReceitasPago.ToString("n2"),
                        totalDespesasPago = totalDespesasPago.ToString("n2"),
                        totalLiquidoPago = totalLiquidoPago.ToString("n2"),
                        totalReceitasPendentes = totalReceitasPendentes.ToString("n2"),
                        totalDespesasPendentes = totalDespesasPendentes.ToString("n2"),
                        totalLiquidoPendentes = totalLiquidoPendentes.ToString("n2"),
                        totalDescontosPago = totalDescontosPago.ToString("n2"),
                        totalDescontosPendentes = totalDescontosPendentes.ToString("n2"),
                        totalLiquidoDescontos = totalLiquidoDescontos.ToString("n2"),
                        totalReceitaDescontos = totalReceitaDescontos.ToString("n2"),
                        totalDespesasDescontos = totalDespesasDescontos.ToString("n2"),
                        totalReceitas = totalReceitas.ToString("n2"),
                        totalDespesas = totalDespesas.ToString("n2"),
                        totalLiquido = totalLiquido.ToString("n2"),
                        lancamentos = dadosFiltrados.Select(i => new
                        {
                            i.Id,
                            tipo = i.Tipo.Capitalize(),
                            cliente = i.Clientes.Nome.Capitalize(),
                            i.Descricao,
                            vencimento = i.DataVencimento.ToString("dd/MM/yyyy"),
                            status = i.Baixado == 0 ? "Pendente" : "Pago",
                            formaPgto = i.FormaPgto ?? "Dinheiro",
                            observacoes = i.Observacoes ?? "",
                            valor = i.Valor.ToString("n2"),
                            desconto = i.Desconto.ToString("n2"),
                            valorTotal = i.ValorDesconto.ToString("n2")
                        })
                    });

                }

                return Json(new { success = false, message = "Erro ao consultar SQL!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Permissao("aLancamento", "Você não tem permissão para adicionar lancamentos")]
        [HttpPost("adicionar-lancamento")]
        public async Task<IActionResult> AdicionarLancamento([FromBody] LancamentoModelRequest lancamento)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Formulário incompleto!" });
            }

            decimal desconto = lancamento.Desconto ?? 0;

            if (desconto > lancamento.Valor)
            {
                return Json(new { success = false, message = "Desconto não pode ser maior que valor da receita/despesa." });
            }

            decimal valordesconto = lancamento.Valor - desconto;
            int baixado = lancamento.Baixado ? 1 : 0;
            DateTime? datarecebimento = (baixado == 0 ? null : lancamento.DataPagamento);
            string? formapgto = (baixado == 0 ? null : lancamento.FormaPgto);
            int usuarioId = int.TryParse(HttpContext.Session.GetString("UserId"), out usuarioId) ? usuarioId : 0;

            LancamentoModel l = new LancamentoModel
            {
                Tipo = lancamento.Tipo,
                Descricao = lancamento.Descricao,
                ClientesId = lancamento.ClienteId,
                Observacoes = lancamento.Observacoes,
                Valor = lancamento.Valor,
                Desconto = desconto,
                ValorDesconto = valordesconto,
                DataVencimento = lancamento.DataVencimento,
                Baixado = baixado,
                DataPagamento = datarecebimento,
                FormaPgto = formapgto,
                UsuariosId = usuarioId
            };

            await _lancamentoRepositorio.Adicionar(l);

            return Json(new { success = true, message = "Sucesso!" });
        }
    }

    public class FiltroModel
    {
        public string? Periodo { get; set; }
        public DateTime? VencimentoDe { get; set; }
        public DateTime? VencimentoAte { get; set; }
        public string? Tipo { get; set; }
        public int? Status { get; set; }
        public int? ClienteId { get; set; }
    }

    public class LancamentoModelRequest
    {
        public int? Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public int ClienteId { get; set; }
        public string? Observacoes { get; set; }
        public decimal Valor { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? ValorDesconto { get; set; }
        public DateTime DataVencimento { get; set; } = DateTime.Now;
        public bool Baixado { get; set; } = false;
        public DateTime? DataPagamento { get; set; }
        public string? FormaPgto { get; set; }
        public string? QtdParcelas { get; set; }
        public decimal? ValorEntrada { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataBasePgto { get; set; }
        public decimal? ValorParcelas { get; set; }
    }
}
