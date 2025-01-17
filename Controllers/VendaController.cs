using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.ViewModel;
using System.Drawing;
using System.Globalization;
using System.Net;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using SIGES.Helpers;


namespace SIGES.Controllers
{
    [Authorize]
    [Route("vendas")]
    public class VendaController : Controller
    {
        private readonly IVenda _vendaRepositorio;
        private readonly ILogService _logServiceRepositorio;
        private readonly IItensVenda _itensVendaRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public VendaController(IVenda vendaRepositorio, ILogService logServiceRepositorio, IItensVenda itensVendaRepositorio, IProdutoRepositorio produtoRepositorio)
        {
            _vendaRepositorio = vendaRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
            _itensVendaRepositorio = itensVendaRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var vendas = _vendaRepositorio.Listar();
            var viewModel = new VendaViewModel
            {
                Vendas = vendas.ToList(),
            };

            ViewData["ActivePanel"] = "";

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

            string observacaoSanitizada = HtmlHelpers.HigienizarHtml(venda.Observacao);
            venda.Observacao = observacaoSanitizada;
            observacaoSanitizada = HtmlHelpers.HigienizarHtml(venda.ObservacaoCliente);
            venda.ObservacaoCliente = observacaoSanitizada;

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

            // Carregar os itens da venda
            var itensVenda = _itensVendaRepositorio.Listar()
                .Where(i => i.VendaId == id)
                .ToList();

            ViewData["VendaId"] = venda.Id;
            ViewData["ClienteNome"] = venda.Cliente?.Nome ?? "Cliente não encontrado";
            ViewData["VendedorNome"] = venda.Usuario?.Nome ?? "Usuário não encontrado";

            var viewModel = new VendaViewModel
            {
                Venda = venda,
                Item = new ItensVendaModel(),
                Itens = itensVenda
            };

            return View("EVenda", viewModel);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, VendaModel venda)
        {
            // Carregar os itens da venda
            var itensVenda = _itensVendaRepositorio.Listar()
                .Where(i => i.VendaId == id)
                .ToList();

            ViewData["VendaId"] = venda.Id;
            ViewData["ClienteNome"] = venda.Cliente?.Nome ?? "Cliente não encontrado";
            ViewData["VendedorNome"] = venda.Usuario?.Nome ?? "Usuário não encontrado";

            var viewModel = new VendaViewModel
            {
                Venda = venda,
                Item = new ItensVendaModel(),
                Itens = itensVenda
            };

            if (!ModelState.IsValid)
            {
                return View("EVenda", viewModel);
            }

            var vendaExistente = await _vendaRepositorio.ObterPorId(id);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, vendaExistente, venda);

            vendaExistente.DataVenda = venda.DataVenda;
            vendaExistente.ClienteId = venda.ClienteId;
            vendaExistente.UsuarioId = venda.UsuarioId;
            vendaExistente.Status = venda.Status;
            vendaExistente.GarantiaDias = venda.GarantiaDias;
            vendaExistente.Observacao = HtmlHelpers.HigienizarHtml(venda.Observacao);
            vendaExistente.ObservacaoCliente = HtmlHelpers.HigienizarHtml(venda.ObservacaoCliente);

            await _vendaRepositorio.Atualizar(vendaExistente);

            TempData["Success"] = "Venda atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("adicionarProduto")]
        public async Task<IActionResult> AdicionarProduto(ItensVendaModel item)
        {
            var venda = await _vendaRepositorio.ObterPorId(item.VendaId);
            if (venda == null)
            {
                return Json(new { result = false, messages = "A venda especificada não existe." });
            }

            var produto = await _produtoRepositorio.ObterPorId(item.ProdutoId);
            if (produto == null)
            {
                return Json(new { result = false, messages = "Produto não encontrado." });
            }

            if (item.Quantidade > produto.Estoque)
            {
                return Json(new { result = false, messages = "A quantidade não pode ser maior que o estoque disponível." });
            }

            try
            {
                string precoString = item.Preco.ToString();
                string precoDecimal = precoString.Replace(".", "").Replace(",", ".");

                item.SubTotal = ((decimal.Parse(precoDecimal) * item.Quantidade) / 100).ToString();
                item.Preco = decimal.Parse(precoDecimal) / 100;

                await _itensVendaRepositorio.Adicionar(item);

                produto.Estoque -= int.Parse(item.Quantidade.ToString());
                await _produtoRepositorio.Atualizar(produto);

                // Carregar itens atualizados
                var itensAtualizados = _itensVendaRepositorio.Listar()
                    .Where(i => i.VendaId == item.VendaId)
                    .ToList();


                return Json(new
                {
                    result = true,
                    items = itensAtualizados.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produto.Descricao,
                        i.Quantidade,
                        Preco = Convert.ToDecimal(i.Preco.ToString()).ToString("C"),
                        SubTotal = Convert.ToDecimal((i.Preco * i.Quantidade).ToString()).ToString("C")
                    }),
                    total = Convert.ToDecimal(itensAtualizados.Sum(i => i.Preco * i.Quantidade).ToString()).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Erro ao adicionar produto: {ex.Message}" });
            }
        }

        [HttpPost("removerProduto")]
        public async Task<IActionResult> RemoverProduto(int itemId, int vendaId)
        {
            var item = await _itensVendaRepositorio.ObterPorId(itemId);
            if (item == null)
            {
                return Json(new { result = false, message = "Item não encontrado." });
            }

            try
            {
                // Repor o estoque e remover o item
                await _itensVendaRepositorio.Excluir(itemId);

                // Carregar itens atualizados
                var itensAtualizados = _itensVendaRepositorio.Listar()
                    .Where(i => i.VendaId == item.VendaId)
                    .ToList();

                return Json(new
                {
                    result = true,
                    items = itensAtualizados.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produto.Descricao,
                        i.Quantidade,
                        Preco = Convert.ToDecimal(i.Preco.ToString()).ToString("C"),
                        SubTotal = Convert.ToDecimal((i.Preco * i.Quantidade).ToString()).ToString("C")
                    }),
                    total = Convert.ToDecimal(itensAtualizados.Sum(i => i.Preco * i.Quantidade).ToString()).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Erro ao deletar produto: {ex.Message}" });
            }
        }

        [HttpGet("imprimir/{id}")]
        public async Task<IActionResult> Imprimir(int id, string tipo = "a4")
        {
            var venda = await _vendaRepositorio.ObterPorId(id);
            if (venda == null)
            {
                TempData["Error"] = "Venda não encontrada.";
                return RedirectToAction("Index");
            }

            var itensVenda = _itensVendaRepositorio.Listar()
                .Where(i => i.VendaId == id)
                .ToList();

            var emitente = await _vendaRepositorio.ObterEmitente();

            var viewModel = new VendaImprimirViewModel
            {
                Venda = venda,
                ItensVenda = itensVenda,
                Emitente = emitente
            };

            if (tipo.ToLower() == "nao-fiscal")
            {
                return View("ImprimirNaoFiscal", viewModel);
            }

            return View("Imprimir", viewModel);
        }

        [HttpGet("visualizar/{id}")]
        public async Task<IActionResult> Visualizar(int id)
        {
            var venda = await _vendaRepositorio.ObterPorId(id);
            if (venda == null)
            {
                TempData["Error"] = "Venda não encontrada.";
                return RedirectToAction("Index");
            }

            string conteudoDecodificado = HtmlHelpers.RemoverTagsHtml(venda.Observacao);
            ViewData["VendaObservacao"] = conteudoDecodificado;

            var viewModel = new VendaViewModel
            {
                Vendas = _vendaRepositorio.Listar().Where((x => x.Id == id)).ToList(),
                Venda = venda,
                Item = new ItensVendaModel(),
                Itens = _itensVendaRepositorio.Listar().Where(i => i.VendaId == id).ToList()
            };

            return View("VVenda", viewModel);
        }

        
    }
}
