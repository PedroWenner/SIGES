using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
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

        public LancamentoController(IRepository<LancamentoModel> repositorio)
        {
            _lancamentoRepository = repositorio;
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
                var dadosFiltrados = await _lancamentoRepository.Listar();
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
                }

                return Json(new { success = true, data = "dadosFiltrados" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
}
