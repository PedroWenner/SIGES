using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("usuarios")]
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
        public async Task<IActionResult> Index()
        {
            var usuarios = _usuarioRepositorio.Listar();
            var usuarioViewModel = new UsuarioViewModel
            {
                Usuarios = usuarios.ToList()
            };

            return View("LUsuario", usuarioViewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            var permissoes = _permissaoRepositorio.Listar().AsQueryable();
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
                return View("AUsuario");
            }

            if (imagemUsuario != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemUsuario.CopyToAsync(memoryStream);
                        usuario.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("AUsuario", usuario);
                }
            }

            await _usuarioRepositorio.Adicionar(usuario);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, usuario);

            TempData["Success"] = "Usuário cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var permissoes = _permissaoRepositorio.Listar().AsQueryable();
            var usuario = await _usuarioRepositorio.ObterPorId(id);
            var usuarioViewModel = new UsuarioViewModel
            {
                Usuario = usuario,
                Permissoes = permissoes.ToList()
            };

            if (usuario == null)
            {
                TempData["Error"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            return View("EUsuario", usuarioViewModel);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, UsuarioModel usuario, IFormFile? imagemUsuario)
        {
            var usuarioExistente = await _usuarioRepositorio.ObterPorId(id);
            if (usuarioExistente == null)
            {
                TempData["Error"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }
            
            string senhaCript = string.Empty;
            if (string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = usuarioExistente.Senha;
                senhaCript = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                usuario.Senha = senhaCript;
            }
            else
            {
                senhaCript = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                usuario.Senha = senhaCript;
            }

            var permissoes = _permissaoRepositorio.Listar().AsQueryable();
            var usuarioViewModel = new UsuarioViewModel
            {
                Usuario = usuario,
                Permissoes = permissoes.ToList()
            };

            if (!ModelState.IsValid)
            {
                return View("EUsuario", usuarioViewModel);
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, usuarioExistente, usuario);

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
            usuarioExistente.Senha = usuario.Senha;

            if (imagemUsuario != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemUsuario.CopyToAsync(memoryStream);
                        usuarioExistente.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("EUsuario", usuario);
                }
            }

            await _usuarioRepositorio.Atualizar(usuarioExistente);

            TempData["Success"] = "Usuário atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var usuario = await _usuarioRepositorio.ObterPorId(id);
            if (usuario == null)
            {
                TempData["Error"] = "Usuário não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, usuario);
            await _usuarioRepositorio.Excluir(id);

            TempData["Success"] = "Usuário excluído com sucesso!";
            
            return RedirectToAction("Index");
        }
    }
}