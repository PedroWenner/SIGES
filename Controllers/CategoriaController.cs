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

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
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
                categoria.ProximoCodigo = await _categoriaRepositorio.GetMaxId() + 1;
                return View("ACategoria", categoria);
            }

            if (imagemCategoria != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemCategoria.CopyToAsync(memoryStream);
                        categoria.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View(categoria);
                }
            }

            await _categoriaRepositorio.Adicionar(categoria);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, categoria);

            TempData["Success"] = "Categoria cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var categoria = await _categoriaRepositorio.ObterPorId(id);
            if (categoria == null)
            {
                TempData["Error"] = "Categoria não encontrada.";
                return RedirectToAction("Index");
            }

            return View("ECategoria", categoria);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, CategoriaModel categoria, IFormFile? imagemCategoria)
        {
            if (!ModelState.IsValid)
            {
                return View("ECategoria", categoria);
            }

            var categoriaExistente = await _categoriaRepositorio.ObterPorId(id);
            if (categoriaExistente == null)
            {
                TempData["Error"] = "Categoria não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, categoriaExistente, categoria);

            categoriaExistente.Descricao = categoria.Descricao;
            if (imagemCategoria != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemCategoria.CopyToAsync(memoryStream);
                        categoriaExistente.ImagemBlob = memoryStream.ToArray();
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
                ModelState.AddModelError("ImagemBlob", "Uma imagem é obrigatória para a categoria.");
                return View("ECategoria", categoria);
            }

            await _categoriaRepositorio.Atualizar(categoriaExistente);
            TempData["Success"] = "Categoria atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var categoria = await _categoriaRepositorio.ObterPorId(id);
            if (categoria == null)
            {
                TempData["Error"] = "Categoria não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, categoria);
            await _categoriaRepositorio.Excluir(id);

            TempData["Success"] = "Categoria excluída com sucesso!";
            return RedirectToAction("Index");
        }
    }
}