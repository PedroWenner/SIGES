using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.Services;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("emails")]
    public class EmailController : Controller
    {
        private readonly IEmailQueueRepository _emailQueueRepository;
        private readonly IOs _osRepositorio;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;
        private readonly IViewRenderService _viewRenderService;
        public EmailController(IEmailQueueRepository emailQueueRepository, IOs osRepositorio, IRepository<EmitenteModel> repositorioEmitente,
            IViewRenderService viewRenderService)
        {
            _emailQueueRepository = emailQueueRepository;
            _osRepositorio = osRepositorio;
            _repositorioEmitente = repositorioEmitente;
            _viewRenderService = viewRenderService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var emails = await _emailQueueRepository.Listar();
            var viewModel = new EmailViewModel
            {
                Emails = emails
            };

            return View("Emails", viewModel);
        }
    }
}
