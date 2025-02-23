using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;
using SIGES.Helpers;
using SIGES.Attributes;
using SIGES.Models.Requests;
using SIGES.Services;


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
        private readonly PaymentGatewaysService _paymentGatewaysService;
        private readonly IPaymentGateway _paymentGateway;

        public VendaController(IVenda vendaRepositorio, ILogService logServiceRepositorio,
            IItensVenda itensVendaRepositorio, IProdutoRepositorio produtoRepositorio,
            PaymentGatewaysService paymentGatewaysService,
            IPaymentGateway paymentGateway)
        {
            _vendaRepositorio = vendaRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
            _itensVendaRepositorio = itensVendaRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _paymentGatewaysService = paymentGatewaysService;
            _paymentGateway = paymentGateway;
        }

        [Permissao("vVenda", "Você não possui permissão para visualizar vendas.")]
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

        [Permissao("aVenda", "Você não possui permissão para adicionar vendas.")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AVenda");
        }

        [Permissao("aVenda", "Você não possui permissão para adicionar vendas.")]
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

        [Permissao("eVenda", "Você não possui permissão para editar vendas.")]
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
            var produtos = _vendaRepositorio.GetProdutos(id);
            var viewModel = new VendaViewModel
            {
                Venda = venda,
                Item = new ItensVendaModel(),
                Itens = produtos.ToList()
            };

            return View("EVenda", viewModel);
        }

        [Permissao("eVenda", "Você não possui permissão para editar vendas.")]
        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, VendaModel venda)
        {
            // Carregar os itens da venda
            var produtos = _vendaRepositorio.GetProdutos(id);

            var viewModel = new VendaViewModel
            {
                Venda = venda,
                Item = new ItensVendaModel(),
                Itens = produtos.ToList()
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

        [Permissao("eVenda", "Você não possui permissão para editar vendas.")]
        [HttpPost("adicionarProduto")]
        public async Task<IActionResult> AdicionarProduto(ItensVendaModel item)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, messages = "Validar todos os campos antes de adicionar produto." });
            }

            var venda = await _vendaRepositorio.ObterPorId(item.VendaId);
            if (venda == null)
            {
                return Json(new { result = false, messages = "A venda especificada não existe." });
            }

            if (venda.Faturado == 1)
            {
                return Json(new { result = false, messages = "Não é possivel adicionar produtos. Venda já faturada!" });
            }

            var produto = await _produtoRepositorio.ObterPorId(item.ProdutoId);
            if (produto == null)
            {
                return Json(new { result = false, messages = "Produto não encontrado." });
            }

            if (ConfiguracaoHelper.ObterValor("control_estoque") == "1")
            {
                if (item.Quantidade > produto.Estoque)
                {
                    return Json(new { result = false, messages = "A quantidade não pode ser maior que o estoque disponível." });
                }
            }

            try
            {
                item.SubTotal = item.Preco * item.Quantidade;
                await _itensVendaRepositorio.Adicionar(item);

                produto.Estoque -= item.Quantidade;
                await _produtoRepositorio.Atualizar(produto);

                var produtos = _vendaRepositorio.GetProdutos(venda.Id);

                venda.Desconto = 0;
                venda.ValorDesconto = 0;
                venda.ValorTotal = produtos.Sum(x => x.Preco * x.Quantidade);
                await _vendaRepositorio.Atualizar(venda);

                return Json(new
                {
                    result = true,
                    items = produtos.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produto.Descricao,
                        i.Quantidade,
                        Preco = i.Preco.ToString("N2"),
                        SubTotal = i.SubTotal?.ToString("N2")
                    }),
                    total = produtos.Sum(x => x.SubTotal)?.ToString("N2")
                });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Erro ao adicionar produto: {ex.Message}" });
            }
        }

        [Permissao("eVenda", "Você não possui permissão para editar vendas.")]
        [HttpPost("removerProduto")]
        public async Task<IActionResult> RemoverProduto(int itemId, int vendaId)
        {
            var venda = await _vendaRepositorio.ObterPorId(vendaId);
            if (venda == null)
            {
                return Json(new { result = false, message = "A venda especificada não existe." });
            }

            if (venda.Faturado == 1)
            {
                return Json(new { result = false, message = "Não é possivel excluir produtos. Venda já faturada!" });
            }

            var item = await _itensVendaRepositorio.ObterPorId(itemId);
            if (item == null)
            {
                return Json(new { result = false, message = "Item não encontrado." });
            }

            try
            {
                // Repor o estoque e remover o item
                await _itensVendaRepositorio.Excluir(itemId);

                var produtos = _vendaRepositorio.GetProdutos(venda.Id);

                venda.Desconto = 0;
                venda.ValorDesconto = 0;
                venda.ValorTotal = produtos.Sum(x => x.Preco * x.Quantidade);
                await _vendaRepositorio.Atualizar(venda);

                return Json(new
                {
                    result = true,
                    items = produtos.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produto.Descricao,
                        i.Quantidade,
                        Preco = i.Preco.ToString("N2"),
                        SubTotal = i.SubTotal?.ToString("N2")
                    }),
                    total = produtos.Sum(x => x.SubTotal)?.ToString("N2")
                });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Erro ao deletar produto: {ex.Message}" });
            }
        }

        [Permissao("vVenda", "Você não possui permissão para visualizar vendas.")]
        [HttpGet("imprimir/{id}")]
        public async Task<IActionResult> Imprimir(int id, string tipo = "a4")
        {
            var venda = await _vendaRepositorio.ObterPorId(id);
            if (venda == null)
            {
                TempData["Error"] = "Venda não encontrada.";
                return RedirectToAction("Index");
            }

            var emitente = await _vendaRepositorio.ObterEmitente();
            if (emitente == null)
            {
                TempData["Error"] = "Emitente não configurado.";
                return RedirectToAction("Index");
            }

            var itensVenda = _itensVendaRepositorio.Listar().Where(i => i.VendaId == id).ToList();
            var podeGerarPix = _paymentGatewaysService.GetGateways()["MercadoPago"].Credentials["access_token"].ToString();
            string qrCode = string.Empty;
            string qrCodeBase64 = string.Empty;

            if (!string.IsNullOrEmpty(podeGerarPix) || tipo.ToLower() != "nao-fiscal")
            {
                PaymentRequest request = new PaymentRequest
                {
                    Id = venda.Id,
                    Tipo = "venda",
                    TransactionAmount = itensVenda.Sum(i => i.Quantidade * i.Preco),
                    Description = $"Venda {id}",
                    PaymentMethodId = "pix",
                    Email = venda.Cliente?.Email ?? "",
                    Identification = venda.Cliente?.Documento?.Replace(".", "").Replace("-", "") ?? ""
                };

                var pix = await _paymentGateway.GerarPix(request);
                if (pix is not null)
                {
                    qrCode = pix.GetType().GetProperty("qrCode")?.GetValue(pix)?.ToString() ?? string.Empty;
                    qrCodeBase64 = pix.GetType().GetProperty("qrCodeBase64")?.GetValue(pix)?.ToString() ?? string.Empty;
                }
            }

            var viewModel = new VendaImprimirViewModel
            {
                Venda = venda,
                ItensVenda = itensVenda,
                Emitente = emitente,
                QrCodeBase64 = qrCodeBase64,
                ChavePixFormatada = ""
            };

            if (tipo.ToLower() == "nao-fiscal")
            {
                return View("ImprimirNaoFiscal", viewModel);
            }

            return View("Imprimir", viewModel);
        }

        [Permissao("vVenda", "Você não possui permissão para visualizar vendas.")]
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
                Itens = _vendaRepositorio.GetProdutos(id).ToList()
            };

            return View("VVenda", viewModel);
        }

        [Permissao("eVenda", "Você não possui permissão para editar vendas.")]
        [HttpPost("adicionarDesconto")]
        public async Task<IActionResult> AdicionarDesconto(VendaModel venda)
        {
            var vendaExistente = await _vendaRepositorio.ObterPorId(venda.Id);
            if (vendaExistente == null)
            {
                return Json(new { sucesso = false, message = "Venda não encontrada." });
            }

            if (vendaExistente.Faturado == 1)
            {
                return Json(new { sucesso = false, message = "Não é possivel adicionar desconto. Venda já faturada!" });
            }

            try
            {
                var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
                await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, vendaExistente, venda);

                vendaExistente.Desconto = venda.Desconto;
                vendaExistente.TipoDesconto = venda.TipoDesconto;
                vendaExistente.ValorDesconto = venda.ValorDesconto;
                await _vendaRepositorio.Atualizar(vendaExistente);

                return Json(new
                {
                    sucesso = true,
                    message = "Desconto adicionado com sucesso!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, message = $"Erro ao adicionar desconto ao produto: {ex.Message}" });
            }
        }

        [HttpGet("obterItensVenda")]
        public async Task<IActionResult> ObterItensVenda(int vendaId)
        {
            var venda = await _vendaRepositorio.ObterPorId(vendaId);
            if (venda == null)
            {
                return Json(new { result = false, message = "Venda não encontrada." });
            }

            var produtos = _vendaRepositorio.GetProdutos(venda.Id).ToList();

            string total = produtos.Sum(i => i.SubTotal ?? 0).ToString("n2");
            string desconto = (venda.Desconto ?? 0).ToString("n2");
            string tipoDesconto = venda.TipoDesconto ?? "real";
            string totalDesconto = (venda.ValorDesconto ?? 0).ToString("n2"); //ValorComDesconto(total, desconto, tipoDesconto);

            if (tipoDesconto == "real")
            {
                desconto = "R$ " + desconto;
            }
            else
            {
                desconto = desconto + "%";
            }

            return Json(new
            {
                result = true,
                produtos = produtos.Select(i => new
                {
                    i.Id,
                    ProdutoNome = i.Produto.Descricao,
                    i.Quantidade,
                    Preco = i.Preco.ToString("n2"),
                    SubTotal = i.SubTotal?.ToString("n2")
                }),
                total,
                desconto,
                totalDesconto
            });
        }

        public decimal ValorComDesconto(decimal valor, decimal desconto, string tipoDesconto)
        {
            decimal defDesconto = 0;

            if (desconto > 0)
            {
                if (tipoDesconto == "porcento")
                {
                    defDesconto = desconto * (valor) / 100;
                }
                else if (tipoDesconto == "real")
                {
                    defDesconto = desconto;
                }
                else
                {
                    defDesconto = 0;
                }
            }
            
            return (valor) - defDesconto;
        }

        [Permissao("dVenda", "Você não possui permissão para excluir vendas.")]
        [HttpPost("excluir/{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var venda = await _vendaRepositorio.ObterPorId(id);
                if (venda == null)
                {
                    return Json(new { result = false, message = "Venda não encontrada." });
                }

                var cobrancas = _vendaRepositorio.GetCobrancas(id).Where(c => c.Status == "approved");
                if (cobrancas.Any())
                {
                    return Json(new { result = false, message = "Não é possível excluir essa venda. Já existe pagamento confirmado." });
                }

                // aqui é onde ele exclui os produtos vinculados a venda
                await _vendaRepositorio.ExcluirProdutos(venda.Id);
                // aqui é onde ele exclui o lançamento vinculado a venda
                await _vendaRepositorio.ExcluirLancamento(venda.Id);
                // aqui é onde ele exclui as cobranças vinculados a venda
                await _vendaRepositorio.ExcluirCobrancas(venda.Id);
                // aqui é onde ele exclui a venda
                await _vendaRepositorio.Excluir(venda.Id);

                return Json(new { result = true, message = "Venda removida com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
    }
}