using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;

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

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AProduto");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(ProdutoModel produto, IFormFile? imagemProduto)
        {
            if (!ModelState.IsValid)
            {
                return View("AProduto", produto);
            }

            // Processar a imagem como blob
            if (imagemProduto != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemProduto.CopyToAsync(memoryStream);
                        produto.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View(produto);
                }
            }

            await _produtoRepositorio.Adicionar(produto);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, produto);

            TempData["Success"] = "Produto cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtoRepositorio.ObterPorId(id);
            if (produto == null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return RedirectToAction("Index");
            }

            return View("EProduto", produto);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, ProdutoModel produto, IFormFile? imagemProduto)
        {
            if (!ModelState.IsValid)
            {
                return View("EProduto", produto);
            }

            var produtoExistente = await _produtoRepositorio.ObterPorId(id);
            if (produtoExistente == null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, produtoExistente, produto);

            produtoExistente.CodigoBarras = produto.CodigoBarras;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.TipoMovimento = produto.TipoMovimento;
            produtoExistente.PrecoCompra = produto.PrecoCompra;
            produtoExistente.MargemLucro = produto.MargemLucro;
            produtoExistente.PrecoVenda = produto.PrecoVenda;
            produtoExistente.UnidadeMedida = produto.UnidadeMedida;
            produtoExistente.Estoque = produto.Estoque;
            produtoExistente.EstoqueMin = produto.EstoqueMin;
            if (imagemProduto != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemProduto.CopyToAsync(memoryStream);
                        produtoExistente.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View(produtoExistente);
                }
            }

            await _produtoRepositorio.Atualizar(produtoExistente);
            TempData["Success"] = "Produto atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
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

        [HttpPost("tualizarEstoque")]
        public async Task<IActionResult> AtualizarEstoque(int id, ProdutoModel produto)
        {
            var produtoExistente = await _produtoRepositorio.ObterPorId(id);
            if (produtoExistente == null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, produtoExistente, produto, new List<string> { "Estoque" });

            produtoExistente.Estoque = produto.Estoque;
            await _produtoRepositorio.Atualizar(produtoExistente);

            TempData["Success"] = "Estoque atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("listarProdutos")]
        public JsonResult ListarProdutos(string term)
        {
            var produtos = _produtoRepositorio.Listar()
                .Where(c => c.Descricao.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(c => new
                {
                    value = c.Id,
                    nome = c.Descricao,
                    preco = c.PrecoVenda,
                    estoque = c.Estoque
                }).ToList();

            return Json(produtos);
        }
    }
}
