using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Data;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("garantias")]
    public class GarantiaController : Controller
    {
        private readonly IGarantia _garantiaRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public GarantiaController(IGarantia garantiaRepositorio, ILogService logServiceRepositorio)
        {
            _garantiaRepositorio = garantiaRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var garantias = _garantiaRepositorio.Listar();
            var viewModel = new GarantiaViewModel
            {
                Garantias = garantias.ToList()
            };

            return View("LGarantia", viewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AGarantia");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(GarantiaModel garantia)
        {
            if (!ModelState.IsValid)
            {
                return View("AGarantia", garantia);
            }

            await _garantiaRepositorio.Adicionar(garantia);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, garantia);

            TempData["Success"] = "Garantia cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var garantia = await _garantiaRepositorio.ObterPorId(id);
            if (garantia == null)
            {
                TempData["Error"] = "Garantia não encontrada.";
                return RedirectToAction("Index");
            }

            var viewModel = new GarantiaViewModel
            {
                Garantia = garantia
            };

            return View("EGarantia", viewModel);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, GarantiaModel garantia)
        {
            if (!ModelState.IsValid)
            {
                return View("EGarantia", garantia);
            }

            var existente = await _garantiaRepositorio.ObterPorId(id);
            if (existente == null)
            {
                TempData["Error"] = "Garantia não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, existente, garantia);

            existente.DataGarantia = garantia.DataGarantia;
            existente.RefGarantia = garantia.RefGarantia;
            existente.TextoGarantia = garantia.TextoGarantia;
            existente.UsuarioId = garantia.UsuarioId;

            await _garantiaRepositorio.Atualizar(existente);

            TempData["Success"] = "Garantia atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var garantia = await _garantiaRepositorio.ObterPorId(id);
            if (garantia == null)
            {
                TempData["Error"] = "Garantia não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, garantia);
            await _garantiaRepositorio.Excluir(id);

            TempData["Success"] = "Garantia excluída com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("imprimir/{id}")]
        public async Task<IActionResult> Imprimir(int id, string tipo = "termo-garantia")
        {
            var garantia = await _garantiaRepositorio.ObterPorId(id);
            if (garantia == null)
            {
                TempData["Error"] = "Termo não encontrado.";
                return RedirectToAction("Index");
            }

            var emitente = await _garantiaRepositorio.ObterEmitente();

            var viewModel = new GarantiaImprimirViewModel
            {
                Garantia = garantia,
                Emitente = emitente
            };

            if (tipo.ToLower() == "termo-garantia")
            {
                return View("ImprimirTermoGarantia", viewModel);
            }

            return View("Imprimir", viewModel);
        }
    }
}