using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;
using System.Text.Json;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("permissoes")]
    public class PermissaoController : Controller
    {
        private readonly IPermissaoRepositorio _permissaoRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public PermissaoController(IPermissaoRepositorio permissaoRepositorio, ILogService logServiceRepositorio)
        {
            _permissaoRepositorio = permissaoRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var permissoes = _permissaoRepositorio.Listar();
            var permissaoViewModel = new PermissaoViewModel
            {
                Permissoes = permissoes.ToList(),
            };

            return View("LPermissao", permissaoViewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("APermissao");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(PermissaoModel permissao)
        {
            if (!ModelState.IsValid)
            {
                return View("APermissao", permissao);
            }

            // Coleta os checkboxes selecionados
            var permissoesDisponiveis = new List<string>();
            foreach (var key in Request.Form.Keys)
            {
                if (Request.Form[key].Count > 0 && Request.Form[key].ToString() == "1")
                {
                    permissoesDisponiveis.Add(key);
                }
            }

            // Cria uma lista de permissões atualizada
            var permissaoDetalhes = new List<PermissaoDetalheModel>();
            foreach (var permissaoNome in permissoesDisponiveis)
            {
                var valorCheckbox = Request.Form[permissaoNome].Count > 0;
                permissaoDetalhes.Add(new PermissaoDetalheModel
                {
                    NomePermissao = permissaoNome,
                    Ativo = valorCheckbox ? 1 : 0
                });
            }

            // Atualiza os dados da categoria
            permissao.Nome = permissao.Nome;
            permissao.Permissoes = JsonSerializer.Serialize(permissaoDetalhes);
            permissao.Situacao = 1;
            permissao.Data = DateTime.Now;

            await _permissaoRepositorio.Adicionar(permissao);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, permissao);

            TempData["Success"] = "Permissão cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var permissao = await _permissaoRepositorio.ObterPorId(id);
            if (permissao == null)
            {
                TempData["Error"] = "Permissão não encontrada.";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(permissao.Permissoes))
            {
                var permissoes = JsonSerializer.Deserialize<List<PermissaoDetalheModel>>(permissao.Permissoes);
                foreach (var permissaoDetalhe in permissoes)
                {
                    ViewData[permissaoDetalhe.NomePermissao] = permissaoDetalhe.Ativo == 1 ? "checked" : "";
                }
            }

            return View("EPermissao", permissao);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, PermissaoModel permissao)
        {
            if (!ModelState.IsValid)
            {
                return View("EPermissao", permissao);
            }

            var permissaoExistente = await _permissaoRepositorio.ObterPorId(id);
            if (permissaoExistente == null)
            {
                TempData["Error"] = "Permissão não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, permissaoExistente, permissao);
            
            var permissoesDisponiveis = new List<string>();
            foreach (var key in Request.Form.Keys)
            {
                if (Request.Form[key].Count > 0 && Request.Form[key].ToString() == "1")
                {
                    permissoesDisponiveis.Add(key);
                }
            }

            var permissaoDetalhes = new List<PermissaoDetalheModel>();
            foreach (var permissaoNome in permissoesDisponiveis)
            {
                var valorCheckbox = Request.Form[permissaoNome].Count > 0;
                permissaoDetalhes.Add(new PermissaoDetalheModel
                {
                    NomePermissao = permissaoNome,
                    Ativo = valorCheckbox ? 1 : 0
                });
            }

            permissaoExistente.Nome = permissao.Nome;
            permissaoExistente.Permissoes = JsonSerializer.Serialize(permissaoDetalhes);
            permissaoExistente.Situacao = permissao.Situacao;

            await _permissaoRepositorio.Atualizar(permissaoExistente);

            TempData["Success"] = "Permissão atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var permissao = await _permissaoRepositorio.ObterPorId(id);
            if (permissao == null)
            {
                TempData["Error"] = "Permissão não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, permissao);
            await _permissaoRepositorio.Excluir(id);

            TempData["Success"] = "Permissão excluída com sucesso!";

            return RedirectToAction("Index");
        }
    }
}
