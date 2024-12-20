using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("vendas")]
    public class VendaController : Controller
    {
        private readonly IVenda _vendaRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public VendaController(IVenda vendaRepositorio, ILogService logServiceRepositorio)
        {
            _vendaRepositorio = vendaRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var vendas = _vendaRepositorio.Listar();
            var viewModel = new VendaViewModel
            {
                Vendas = vendas.ToList(),
            };

            return View("LVenda", viewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AVenda");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(VendaModel venda)
        {
            if (!ModelState.IsValid)
            {
                return View("AVenda");
            }

            await _vendaRepositorio.Adicionar(venda);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, venda);

            TempData["Success"] = "Venda cadastrada com sucesso!";
            return RedirectToAction("Editar", new { id = venda.Id });
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var venda = await _vendaRepositorio.ObterPorId(id);
            if (venda == null)
            {
                TempData["Error"] = "Venda não encontrada.";
                return RedirectToAction("Index");
            }

            ViewData["VendaId"] = venda.Id;
            ViewData["ClienteNome"] = venda.Cliente?.Nome ?? "Cliente não encontrado";
            ViewData["VendedorNome"] = venda.Usuario?.Nome ?? "Usuário não encontrado";

            return View("EVenda", venda);
        }
    }
}
