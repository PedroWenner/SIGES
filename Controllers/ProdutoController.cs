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
    [Route("produtos")]
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio, ILogService logServiceRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var produtos = _produtoRepositorio.Listar();
            var viewModel = new ProdutoViewModel
            {
                Produtos = produtos.ToList()
            };

            return View("LProduto", viewModel);
        }

        [Permissao("aProduto")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AProduto");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(ProdutoModel produto)
        {
            if (!ModelState.IsValid)
            {
                return View("AProduto", produto);
            }

            // Salva no banco de dados
            await _produtoRepositorio.Adicionar(produto);

            // Recupera o nome do usuário da sessão
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";

            // Registrar log de adição
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, produto);

            // Redireciona com mensagem de sucesso
            TempData["Success"] = "Produto cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [Permissao("eProduto")]
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca pelo ID
            var produto = await _produtoRepositorio.ObterPorId(id);

            if (produto == null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return RedirectToAction("Index");
            }

            // Retorna a View com o modelo populado
            return View("EProduto", produto);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, ProdutoModel produto)
        {
            if (!ModelState.IsValid)
            {
                return View("EProduto", produto);
            }

            // Busca a categoria existente
            var produtoExistente = await _produtoRepositorio.ObterPorId(id);

            if (produtoExistente == null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, produtoExistente, produto);

            produtoExistente.Codigo = produto.Codigo;
            produtoExistente.Status = produto.Status;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Peso = produto.Peso;
            produtoExistente.UnidadeMedida = produto.UnidadeMedida;
            produtoExistente.CategoriaId = produto.CategoriaId;
            produtoExistente.Valor = produto.Valor;
            produtoExistente.Observacao = produto.Observacao;
            produtoExistente.ImagemBlob = produto.ImagemBlob;

            // Salva as alterações
            await _produtoRepositorio.Atualizar(produtoExistente);

            TempData["Success"] = "Produto atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            // Busca a categoria pelo ID
            var produto = await _produtoRepositorio.ObterPorId(id);

            if (produto == null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, produto);
            await _produtoRepositorio.Excluir(id);

            TempData["Success"] = "Produto excluído com sucesso!";

            return RedirectToAction("Index");
        }
    }
}
