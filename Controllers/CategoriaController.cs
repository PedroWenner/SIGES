using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("categorias")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public CategoriaController(ICategoriaRepositorio categoriaRepositorio, ILogService logServiceRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var categorias = _categoriaRepositorio.Listar();
            var categoriaViewModel = new CategoriaViewModel
            {
                Categorias = categorias.ToList(),
            };

            return View("LCategoria", categoriaViewModel);
        }

        [Permissao("aCategoria")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            // Obtém o próximo código
            var proximoCodigo = await _categoriaRepositorio.GetMaxId() + 1;

            var model = new CategoriaModel
            {
                ProximoCodigo = proximoCodigo
            };

            return View("ACategoria", model);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(CategoriaModel categoria, IFormFile? imagemCategoria)
        {
            if (!ModelState.IsValid)
            {
                // Recarrega o próximo código caso o ModelState falhe
                categoria.ProximoCodigo = await _categoriaRepositorio.GetMaxId() + 1;
                return View("ACategoria", categoria);
            }

            // Processar a imagem como blob
            if (imagemCategoria != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemCategoria.CopyToAsync(memoryStream);
                        categoria.ImagemBlob = memoryStream.ToArray(); // Salva o blob no modelo
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View(categoria);
                }
            }

            // Salva a categoria no banco de dados
            await _categoriaRepositorio.Adicionar(categoria);

            // Recupera o nome do usuário da sessão
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Registrar log de adição
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, categoria);

            // Redireciona com mensagem de sucesso
            TempData["Success"] = "Categoria cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [Permissao("eCategoria")]
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca a categoria pelo ID
            var categoria = await _categoriaRepositorio.ObterPorId(id);

            if (categoria == null)
            {
                TempData["Error"] = "Categoria não encontrada.";
                return RedirectToAction("Index");
            }

            // Retorna a View com o modelo populado
            return View("ECategoria", categoria);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, CategoriaModel categoria, IFormFile? imagemCategoria)
        {
            if (!ModelState.IsValid)
            {
                return View("ECategoria", categoria);
            }

            // Busca a categoria existente
            var categoriaExistente = await _categoriaRepositorio.ObterPorId(id);

            if (categoriaExistente == null)
            {
                TempData["Error"] = "Categoria não encontrada.";
                return RedirectToAction("Index");
            }

            // Recupera o nome do usuário da sessão
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Registrar log de alterações
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, categoriaExistente, categoria);

            // Atualiza os dados da categoria
            categoriaExistente.Descricao = categoria.Descricao;

            // Verifica se uma nova imagem foi enviada
            if (imagemCategoria != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemCategoria.CopyToAsync(memoryStream);
                        categoriaExistente.ImagemBlob = memoryStream.ToArray(); // Atualiza o blob
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("ECategoria", categoria);
                }
            }
            else if (categoriaExistente.ImagemBlob == null)
            {
                // Se nenhuma imagem foi enviada e nenhuma existe, retorna erro
                ModelState.AddModelError("ImagemBlob", "Uma imagem é obrigatória para a categoria.");
                return View("ECategoria", categoria);
            }

            // Salva as alterações
            await _categoriaRepositorio.Atualizar(categoriaExistente);

            TempData["Success"] = "Categoria atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [Permissao("dCategoria")]
        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            // Busca a categoria pelo ID
            var categoria = await _categoriaRepositorio.ObterPorId(id);

            if (categoria == null)
            {
                TempData["Error"] = "Categoria não encontrada.";
                return RedirectToAction("Index");
            }

            try
            {
                var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

                // Registrar log de exclusão
                await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, categoria);

                // Remove a categoria do banco de dados
                await _categoriaRepositorio.Excluir(id);

                TempData["Success"] = "Categoria excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao excluir a categoria: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}