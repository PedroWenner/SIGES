using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("emails")]
    public class EmailController : Controller
    {
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        public EmailController(IRepository<EmailQueue> emailQueueRepository)
        {
            _emailQueueRepository = emailQueueRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var emails = await _emailQueueRepository.Listar();
            var viewModel = new EmailViewModel
            {
                Emails = emails.ToList()
            };

            return View("Emails", viewModel);
        }

        [Permissao("dEmail", "Você não possui permissão para excluir e-mails.")]
        [HttpPost("excluir/{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var email = await _emailQueueRepository.ObterPorId(id);
                if (email == null)
                {
                    return Json(new { result = false, message = "E-mail não encontrado." });
                }

                // aqui é onde ele exclui o e-mail
                await _emailQueueRepository.Deletar(email);

                return Json(new { result = true, message = "E-mail removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
    }
}
