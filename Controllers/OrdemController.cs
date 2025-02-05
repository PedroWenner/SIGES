using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.Services;
using SIGES.ViewModel;
using System.Text.Json;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("os")]
    public class OrdemController : Controller
    {
        private readonly IOs _osRepositorio;
        private readonly ILogService _logServiceRepositorio;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;
        private readonly IRepository<UsuarioModel> _repositorioUsuario;
        private readonly IConfiguracao _configuracaoRepositorio;
        private readonly IEmailQueueRepository _emailQueueRepository;
        private readonly IViewRenderService _viewRenderService;

        public OrdemController(IOs osRepositorio, ILogService logServiceRepositorio, IRepository<EmitenteModel> repositorioEmitente,
            IRepository<UsuarioModel> repositorioUsuario, IConfiguracao configuracaoRepositorio, IEmailQueueRepository emailQueueRepository,
            IViewRenderService viewRenderService)
        {
            _osRepositorio = osRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
            _repositorioEmitente = repositorioEmitente;
            _repositorioUsuario = repositorioUsuario;
            _configuracaoRepositorio = configuracaoRepositorio;
            _emailQueueRepository = emailQueueRepository;
            _viewRenderService = viewRenderService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var ordens = _osRepositorio.Listar();
            var viewModel = new OsViewModel
            {
                Ordens = ordens.ToList(),
            };

            return View("LOrdem", viewModel);
        }

        [Permissao("aOrdem", "Você não tem permissão para adicionar OS.")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AOrdem");
        }

        [Permissao("aOrdem", "Você não tem permissão para adicionar OS.")]
        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(OsModel os)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = "Error", message = "Favor validar todos os campos obrigatórios." });
            }

            os.faturado = 0;

            try
            {
                await _osRepositorio.Adicionar(os);

                var emitente = await _repositorioEmitente.GetLastRegister();
                var osCadastrada = await _osRepositorio.ObterPorId(os.Id);
                var tecnico = await _repositorioUsuario.ObterPorId(os.UsuariosId);
                var osNotification = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("os_notification");
                var emailAutomatico = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("email_automatico");

                if (osNotification != "nenhum" && emailAutomatico == "1")
                {
                    List<string> remetentes = DefinirRemetentes(osNotification, osCadastrada.Clientes.Email, tecnico.Email, emitente.Email);

                    string status = await EnviarEmail(os.Id, remetentes, "Ordem de Serviço - Criada");
                }
                
                TempData["Success"] = "OS adicionada com sucesso, você pode adicionar produtos ou serviços a essa OS nas abas de produtos e serviços!";
                return RedirectToAction("Editar", new { id = os.Id });

            }
            catch(Exception)
            {
                return Json(new { status = "Error", message = "Ocorreu um erro." });
            }
        }

        [Permissao("eOrdem")]
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca pelo ID
            var os = await _osRepositorio.ObterPorId(id);

            if (os == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            // Retorna a View com o modelo populado
            return View("EOrdem", os);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, OsModel os)
        {
            if (!ModelState.IsValid)
            {
                return View("EOrdem", os);
            }

            // Busca a categoria existente
            var osExistente = await _osRepositorio.ObterPorId(id);

            if (osExistente == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, osExistente, os);

            //servicoExistente.ImagemBlob = produto.ImagemBlob;

            // Salva as alterações
            await _osRepositorio.Atualizar(osExistente);

            TempData["Success"] = "OS atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            // Busca a categoria pelo ID
            var os = await _osRepositorio.ObterPorId(id);

            if (os == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, os);
            await _osRepositorio.Excluir(id);

            TempData["Success"] = "OS excluída com sucesso!";

            return RedirectToAction("Index");
        }

        private List<string> DefinirRemetentes(string configuracao, string emailCliente, string emailTecnico, string emailEmitente)
        {
            var remetentes = new List<string>();

            switch (configuracao)
            {
                case "todos":
                    remetentes.AddRange(new[] { emailCliente, emailTecnico, emailEmitente });
                    break;
                case "cliente":
                    remetentes.Add(emailCliente);
                    break;
                case "tecnico":
                    remetentes.Add(emailTecnico);
                    break;
                case "emitente":
                    remetentes.Add(emailEmitente);
                    break;
                default:
                    remetentes.Add(emailCliente);
                    break;
            }

            return remetentes.Distinct().Where(r => !string.IsNullOrEmpty(r)).ToList();
        }

        [Permissao("vOs", "Você não tem permissão para enviar e-mail!")]
        [HttpPost("enviar-email/{id}")]
        public async Task<string> EnviarEmail(int id, List<string> remetentes, string assunto)
        {
            var os = await _osRepositorio.ObterPorId(id);
            var emitente = await _repositorioEmitente.GetLastRegister();
            if (string.IsNullOrEmpty(emitente.Email))
            {
                return "error";
            }

            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);

            // **Renderizando a View HTML**
            var model = new OsEmailViewModel
            {
                Os = os,
                Emitente = emitente,
                Produtos = produtos.ToList(),
                Servicos = servicos.ToList(),
            };
            var htmlBody = await _viewRenderService.RenderToStringAsync("Email/Os/OsEmail", model);
            //string htmlBody = await viewRenderService.RenderToStringAsync("Email/Os/OsEmail", model);

            foreach (var remetente in remetentes.Where(r => !string.IsNullOrEmpty(r)))
            {
                await _emailQueueRepository.AdicionarEmail(new EmailQueue
                {
                    To = remetente,
                    Message = htmlBody,
                    Status = "pending",
                    Date = DateTime.UtcNow
                });
            }

            return "success";
        }
    }
}
