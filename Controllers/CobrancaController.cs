using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.ViewModel;
    
namespace SIGES.Controllers
{
    [Authorize]
    [Route("cobrancas")]
    public class CobrancaController : Controller
    {
        private readonly ICobranca _cobrancaRepository;

        public CobrancaController(ICobranca cobrancaRepository)
        {
            _cobrancaRepository = cobrancaRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var cobrancas = await _cobrancaRepository.Listar();
            var cobrancasViewModel = new CobrancaViewModel
            {
                Cobrancas = cobrancas.ToList(),
            };

            return View("LCobranca", cobrancasViewModel);
        }
    }
}