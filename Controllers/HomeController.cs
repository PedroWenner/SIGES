using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Services;
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
            var clientes = _clienteRepositorio.Listar();
            var produtos = _produtoRepositorio.Listar();
            var os = _osRepositorio.Listar();

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
