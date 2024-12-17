using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("servicos")]
    public class ServicoController : Controller
    {
        private readonly IServico _servicoRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public ServicoController(IServico servicoRepositorio, ILogService logServiceRepositorio)
        {
            _servicoRepositorio = servicoRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var servicos = _servicoRepositorio.Listar();
            var viewModel = new ServicoViewModel
            {
                Servicos = servicos.ToList()
            };

            return View("LServico", viewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AServico");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(ServicoModel servico)
        {
            if (!ModelState.IsValid)
            {
                return View("AServico", servico);
            }

            await _servicoRepositorio.Adicionar(servico);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, servico);

            TempData["Success"] = "Serviço cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var servico = await _servicoRepositorio.ObterPorId(id);
            if (servico == null)
            {
                TempData["Error"] = "Serviço não encontrado.";
                return RedirectToAction("Index");
            }

            return View("EServico", servico);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, ServicoModel servico)
        {
            if (!ModelState.IsValid)
            {
                return View("EServico", servico);
            }

            var servicoExistente = await _servicoRepositorio.ObterPorId(id);
            if (servicoExistente == null)
            {
                TempData["Error"] = "Serviço não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, servicoExistente, servico);

            servicoExistente.Nome = servico.Nome;
            servicoExistente.Valor = servico.Valor;
            servicoExistente.Descricao = servico.Descricao;

            await _servicoRepositorio.Atualizar(servicoExistente);

            TempData["Success"] = "Serviço atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var servico = await _servicoRepositorio.ObterPorId(id);
            if (servico == null)
            {
                TempData["Error"] = "Serviço não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, servico);
            await _servicoRepositorio.Excluir(id);

            TempData["Success"] = "Serviço excluído com sucesso!";

            return RedirectToAction("Index");
        }
    }
}
