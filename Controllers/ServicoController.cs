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

            // Obter permissões do usuário logado
            var permissoesUsuario = HttpContext.Items["Permissoes"] as List<PermissaoDetalheModel>;
            ViewData["PermissoesUsuario"] = permissoesUsuario;

            return View("LServico", viewModel);
        }

        [Permissao("aServico")]
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

            // Redireciona com mensagem de sucesso
            TempData["Success"] = "Serviço cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [Permissao("eServico")]
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca pelo ID
            var servico = await _servicoRepositorio.ObterPorId(id);

            if (servico == null)
            {
                TempData["Error"] = "Serviço não encontrado.";
                return RedirectToAction("Index");
            }

            // Retorna a View com o modelo populado
            return View("EServico", servico);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, ServicoModel servico)
        {
            if (!ModelState.IsValid)
            {
                return View("EServico", servico);
            }

            // Busca a categoria existente
            var servicoExistente = await _servicoRepositorio.ObterPorId(id);

            if (servicoExistente == null)
            {
                TempData["Error"] = "Serviço não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, servicoExistente, servico);

            //servicoExistente.ImagemBlob = produto.ImagemBlob;

            // Salva as alterações
            await _servicoRepositorio.Atualizar(servicoExistente);

            TempData["Success"] = "Serviço atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            // Busca a categoria pelo ID
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
