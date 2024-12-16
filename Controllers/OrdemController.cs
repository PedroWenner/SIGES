using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
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

        public OrdemController(IOs osRepositorio, ILogService logServiceRepositorio)
        {
            _osRepositorio = osRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
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

        [Permissao("aOrdem")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AOrdem");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(OsModel os)
        {
            if (!ModelState.IsValid)
            {
                return View("AOrdem", os);
            }

            await _osRepositorio.Adicionar(os);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, os);

            // Redireciona com mensagem de sucesso
            TempData["Success"] = "OS cadastrada com sucesso!";
            return RedirectToAction("Index");
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
    }
}
