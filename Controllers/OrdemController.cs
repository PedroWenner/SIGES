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
    [Route("os")]
    public class OrdemController : Controller
    {
        private readonly IOs _osRepositorio;
        private readonly ILogService _logServiceRepositorio;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;
        private readonly IRepository<UsuarioModel> _repositorioUsuario;
        private readonly IConfiguracao _configuracaoRepositorio;
        private readonly IEmailQueueRepository _emailQueueRepository;
        private readonly Interfaces.IViewRenderService _viewRenderService;
        private readonly IRepository<ProdutoModel> _repositorioProdutos;
        private readonly IRepository<ProdutosOs> _repositorioProdutosOs;
        private readonly IRepository<ServicosOs> _repositorioServicosOs;

        public OrdemController(IOs osRepositorio, ILogService logServiceRepositorio, IRepository<EmitenteModel> repositorioEmitente,
            IRepository<UsuarioModel> repositorioUsuario, IConfiguracao configuracaoRepositorio, IEmailQueueRepository emailQueueRepository,
            Interfaces.IViewRenderService viewRenderService, IRepository<ProdutosOs> repositorioProdutosOs, IRepository<ServicosOs> repositorioServicosOs,
            IRepository<ProdutoModel> repositorioProdutos)
        {
            _osRepositorio = osRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
            _repositorioEmitente = repositorioEmitente;
            _repositorioUsuario = repositorioUsuario;
            _configuracaoRepositorio = configuracaoRepositorio;
            _emailQueueRepository = emailQueueRepository;
            _viewRenderService = viewRenderService;
            _repositorioProdutosOs = repositorioProdutosOs;
            _repositorioServicosOs = repositorioServicosOs;
            _repositorioProdutos = repositorioProdutos;
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
                return Json(new { status = "error", message = "Favor validar todos os campos obrigatórios." });
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

                return Json(new { status = "success", message = "OS adicionada com sucesso, você pode adicionar produtos ou serviços a essa OS nas abas de produtos e serviços!", id = os.Id.ToString() });
            }
            catch(Exception)
            {
                return Json(new { status = "error", message = "Ocorreu um erro." });
            }
        }

        [Permissao("eOrdem", "Você não tem permissão para editar OS.")]
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

            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);

            var viewmodel = new OsEmailViewModel
            {
                Os = os,
                Produtos = produtos.ToList(),
                Servicos = servicos.ToList(),
            };

            return View("EOrdem", viewmodel);
        }

        [Permissao("eOrdem", "Você não tem permissão para editar OS.")]
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

        [Permissao("eOs", "Você não tem permissão para enviar mensagens.")]
        [HttpPost("enviar-whatsapp/{id}")]
        public async Task<IActionResult> EnviarWhatsApp(int id)
        {
            var os = await _osRepositorio.ObterPorId(id);
            if (os == null)
            {
                return Json(new { status = "Error", message = "OS não encontrada." });
            }

            var emitente = (await _repositorioEmitente.Listar()).FirstOrDefault();
            var textoBase = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("notifica_whats");

            if (string.IsNullOrEmpty(textoBase))
            {
                return Json(new { status = "error", message = "O texto base para notificação não está configurado." });
            }

            var zapnumber = new string(os.Clientes?.Telefone?.Where(char.IsDigit).ToArray());

            List<string> troca = new List<string>
            {
                os.Clientes?.Nome ?? "",
                os.Id.ToString() ?? "",
                os.Status ?? "",
                "R$ " + (os.Desconto > 0 && os.ValorDesconto > 0
                    ? os.ValorDesconto.ToString("N2")
                    : (os.Produtos?.Sum(x => x.Preco ?? 0) + os.Servicos?.Sum(x => x.Preco ?? 0)).ToString()),
                os.DescricaoProduto ?? "",
                emitente?.Nome ?? "",
                emitente?.Telefone ?? "",
                os.Observacoes ?? "",
                os.Defeito ?? "",
                os.LaudoTecnico ?? "",
                os.DataFinal?.ToString("dd/MM/yyyy") ?? "",
                os.DataInicial?.ToString("dd/MM/yyyy") ?? "",
                $"{os.Garantia} dias"
            };

            string textoDeNotificacao = HtmlHelpers.CriarTextoWhats(textoBase, troca);

            if (!string.IsNullOrEmpty(zapnumber))
            {

                string linkWhatsApp = $"https://wa.me/55{zapnumber}?text={textoDeNotificacao}";
                return Json(new { status = "success", link = linkWhatsApp });
            }

            return Json(new { status = "error", message = "Número de WhatsApp não encontrado para o cliente." });
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("adicionar-produtos")]
        public async Task<IActionResult> AdicionarProdutos(ProdutosOs produtoOs)
        {
            var os = await _osRepositorio.ObterPorId(produtoOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            if (os.faturado == 1)
            {
                return Json(new { status = "error", message = "Não é possivel adicionar produtos. OS já faturada!" });
            }

            produtoOs.SubTotal = (produtoOs.Preco ?? 0) * (produtoOs.Quantidade);

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Validar campos antes de continuar." });
            }

            var produto = await _repositorioProdutos.ObterPorId(produtoOs.ProdutosId);
            if (produto == null)
            {
                return Json(new { status = "error", message = "Produto não encontrado." });
            }

            if (_configuracaoRepositorio.ObterValorConfiguracaoAsync("control_estoque").ToString() == "1")
            {
                if (produtoOs.Quantidade > produto.Estoque)
                {
                    return Json(new { status = "error", message = "A quantidade não pode ser maior que o estoque disponível." });
                }
            }

            try
            {
                await _repositorioProdutosOs.Adicionar(produtoOs);

                if (_configuracaoRepositorio.ObterValorConfiguracaoAsync("control_estoque").ToString() == "1")
                {
                    produto.Estoque = produto.Estoque - produtoOs.Quantidade;
                    await _repositorioProdutos.Atualizar(produto);
                }

                os.Desconto = 0;
                os.ValorDesconto = 0;
                await _osRepositorio.Atualizar(os);

                var produtosAdicionados = await _repositorioProdutosOs.Listar();

                return Json(new 
                {
                    status = "success",
                    produtos = produtosAdicionados.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produtos?.Descricao,
                        i.Quantidade,
                        Preco = i.Preco?.ToString("C"),
                        SubTotal = i.SubTotal.ToString("C")
                    }),
                    total = produtosAdicionados.Sum(i => i.SubTotal).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}