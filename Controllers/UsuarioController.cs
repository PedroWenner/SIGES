using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Classes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("dashboard/usuarios")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ILogService _logServiceRepositorio;
        private readonly IPermissaoRepositorio _permissaoRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, IPermissaoRepositorio permissaoRepositorio, ILogService logServiceRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
            _permissaoRepositorio = permissaoRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? pagina)
        {
            // Carregar os dados para a View
            var usuarios = _usuarioRepositorio.Listar().AsQueryable();
            int registrosPorPagina = 15;

            var usuarioPaginada = await Paginacao<UsuarioModel>.CreateAsync(usuarios, pagina ?? 1, registrosPorPagina);

            // Montar um ViewModel para enviar os dados para a View
            var usuarioViewModel = new UsuarioViewModel
            {
                Usuarios = usuarioPaginada
            };

            return View("LUsuario", usuarioViewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            // Carregar os dados para a View
            var permissoes = _permissaoRepositorio.Listar().AsQueryable();

            // Montar um ViewModel para enviar os dados para a View
            var usuarioViewModel = new UsuarioViewModel
            {
                Usuario = new UsuarioModel(),
                Permissoes = permissoes.ToList()
            };

            return View("AUsuario", usuarioViewModel);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(UsuarioModel usuario, IFormFile? imagemUsuario)
        {
            if (!ModelState.IsValid)
            {
                // Recarrega o próximo código caso o ModelState falhe
                return View("AUsuario");
            }

            // Processar a imagem como blob
            if (imagemUsuario != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemUsuario.CopyToAsync(memoryStream);
                        usuario.ImagemBlob = memoryStream.ToArray(); // Salva o blob no modelo
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("AUsuario", usuario);
                }
            }

            // Salva a categoria no banco de dados
            await _usuarioRepositorio.Adicionar(usuario);

            // Recupera o nome do usuário da sessão
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Registrar log de adição
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, usuario);

            // Redireciona com mensagem de sucesso
            TempData["Success"] = "Usuário cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca pelo ID
            var usuario = await _usuarioRepositorio.ObterPorId(id);

            if (usuario == null)
            {
                TempData["Error"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            // Retorna a View com o modelo populado
            return View("EUsuario", usuario);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, UsuarioModel usuario, IFormFile? imagemUsuario)
        {
            if (!ModelState.IsValid)
            {
                return View("EUsuario", usuario);
            }

            // Busca a usuário existente
            var usuarioExistente = await _usuarioRepositorio.ObterPorId(id);

            if (usuarioExistente == null)
            {
                TempData["Error"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            // Recupera o nome do usuário da sessão
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Registrar log de alterações
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, usuarioExistente, usuario);

            // Atualiza os dados do usuário
            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.RgNumero = usuario.RgNumero;
            usuarioExistente.Cpf = usuario.Cpf;
            usuarioExistente.Telefone = usuario.Telefone;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Cep = usuario.Cep;
            usuarioExistente.Rua = usuario.Rua;
            usuarioExistente.NumeroRua = usuario.NumeroRua;
            usuarioExistente.Bairro = usuario.Bairro;
            usuarioExistente.Cidade = usuario.Cidade;
            usuarioExistente.Estado = usuario.Estado;
            usuarioExistente.DataExpiracao = usuario.DataExpiracao;
            usuarioExistente.Situacao = usuario.Situacao;
            usuarioExistente.PermissaoId = usuario.PermissaoId;

            // Verifica se uma nova imagem foi enviada
            if (imagemUsuario != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemUsuario.CopyToAsync(memoryStream);
                        usuarioExistente.ImagemBlob = memoryStream.ToArray(); // Atualiza o blob
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("EUsuario", usuario);
                }
            }

            // Salva as alterações
            await _usuarioRepositorio.Atualizar(usuarioExistente);

            TempData["Success"] = "Usuário atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            // Busca pelo ID
            var usuario = await _usuarioRepositorio.ObterPorId(id);

            if (usuario == null)
            {
                TempData["Error"] = "Usuário não encontrada.";
                return RedirectToAction("Index");
            }

            try
            {
                var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

                // Registrar log de exclusão
                await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, usuario);

                // Remove do banco de dados
                await _usuarioRepositorio.Excluir(id);

                TempData["Success"] = "Usuário excluído com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao excluir a usuário: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}