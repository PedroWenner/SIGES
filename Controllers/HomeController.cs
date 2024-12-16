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
        private readonly IOs _osRepositorio;

        public HomeController(IClienteRepositorio clienteRepositorio, IProdutoRepositorio produtoRepositorio, IOs osRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _osRepositorio = osRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Saudacao = SaudeHelper.ObterSaudacao();
            ViewBag.NomeUsuario = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Carregar os dados para a View
            var clientes = _clienteRepositorio.Listar();
            var produtos = _produtoRepositorio.Listar();
            var os = _osRepositorio.Listar();

            // Montar um ViewModel para enviar os dados para a View
            var homeViewModel = new HomeViewModel
            {
                Clientes = clientes.ToList(),
                Produtos = produtos.ToList(),
                Os = os.ToList()
            };

            return View(homeViewModel);
        }
    }
}
