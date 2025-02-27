using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
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
        private readonly IRepository<VendaModel> _vendaRepository;
        private readonly IRepository<ServicoModel> _servicoRepository;

        public HomeController(IClienteRepositorio clienteRepositorio, IProdutoRepositorio produtoRepositorio,
            IOs osRepositorio, IRepository<VendaModel> vendaRepository, IRepository<ServicoModel> servicoRepository)
        {
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _osRepositorio = osRepositorio;
            _vendaRepository = vendaRepository;
            _servicoRepository = servicoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = _clienteRepositorio.Listar();
            var produtos = _produtoRepositorio.Listar();
            var os = _osRepositorio.Listar();
            var vendas = await _vendaRepository.Listar();
            var servicos = await _servicoRepository.Listar();

            var homeViewModel = new HomeViewModel
            {
                Clientes = clientes.ToList(),
                Produtos = produtos.ToList(),
                Os = os.ToList(),
                Vendas = vendas.ToList(),
                Servicos = servicos.ToList()
            };

            return View(homeViewModel);
        }
    }
}
