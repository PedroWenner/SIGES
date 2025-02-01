using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Helpers;
using SIGES.Interfaces;
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
        public EmailController(IEmailQueueRepository emailQueueRepository)
        {
            _emailQueueRepository = emailQueueRepository;
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
