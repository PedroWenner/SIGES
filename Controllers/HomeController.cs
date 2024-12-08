using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
using System.Diagnostics;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public HomeController(IClienteRepositorio clienteRepositorio, IProdutoRepositorio produtoRepositorio, IPedidoRepositorio pedidoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Saudacao = SaudeHelper.ObterSaudacao();
            ViewBag.NomeUsuario = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Carregar os dados para a View
            var clientes = await _clienteRepositorio.ListarTodos();
            var produtos = await _produtoRepositorio.Listar();
            var pedidos = await _pedidoRepositorio.ListarTodos();

            // Montar um ViewModel para enviar os dados para a View
            var homeViewModel = new HomeViewModel
            {
                Clientes = clientes,
                Produtos = produtos,
                Pedidos = pedidos
            };

            return View(homeViewModel);
        }
    }
}
