using iText.Barcodes;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using PdfImage = iText.Layout.Element.Image;
using Microsoft.AspNetCore.Authorization;
using SIGES.Attributes;
using Path = System.IO.Path;
using SIGES.Helpers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace SIGES.Controllers
{
    [Route("relatorios")]
    [Authorize]
    public class RelatorioController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IOs _repositorioOs;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public RelatorioController(IProdutoRepositorio produtoRepositorio, IOs repositorioOs, IRepository<EmitenteModel> repositorioEmitente,
            IConfiguration configuration, HttpClient httpClient, ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _produtoRepositorio = produtoRepositorio;
            _repositorioOs = repositorioOs;
            _repositorioEmitente = repositorioEmitente;
            _configuration = configuration;
            _httpClient = httpClient;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        [HttpGet("ProdutosEtiquetas")]
        public async Task<IActionResult> ProdutosEtiquetas(int deId, int ateId, bool qtdEtiqueta, string etiquetaCode)
        {
            if (deId > ateId)
            {
                TempData["Error"] = "O campo 'De' não pode ser maior que o campo 'Até'.";
                return RedirectToAction("Index", "Produto");
            }

            var produtos = await _produtoRepositorio.ObterPorIntervalo(deId, ateId);

            if (!produtos.Any())
            {
                TempData["Error"] = "Nenhum produto encontrado no intervalo especificado.";
                return RedirectToAction("Index", "Produto");
            }

            try
            {
                var etiquetas = GerarEtiquetas(produtos, qtdEtiqueta, etiquetaCode);

                // Retorna o PDF gerado para o navegador
                return File(etiquetas, "application/pdf", $"etiquetas_{deId}_{ateId}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao gerar etiquetas: {ex.Message}";
                return RedirectToAction("Index", "Produto");
            }
        }

        private byte[] GerarEtiquetas(List<ProdutoModel> produtos, bool qtdEtiqueta, string etiquetaCode)
        {
            using (var ms = new MemoryStream())
            {
                // Cria o PDF
                var pdfWriter = new PdfWriter(ms);
                var pdfDocument = new PdfDocument(pdfWriter);
                var document = new Document(pdfDocument, PageSize.A4);

                foreach (var produto in produtos)
                {
                    var qtd = qtdEtiqueta ? produto.Estoque : 1;

                    for (int i = 0; i < qtd; i++)
                    {
                        // Adiciona os detalhes do produto
                        document.Add(new Paragraph($"Produto: {produto.Descricao}").SetFontSize(12));
                        document.Add(new Paragraph($"Código: {produto.Id}").SetFontSize(12));
                        document.Add(new Paragraph($"Preço: R$ {produto.PrecoVenda:F2}").SetFontSize(12));

                        // Gera o código de barras com base no formato
                        PdfImage barcodeImage = GerarCodigoDeBarras(produto.CodigoBarras, etiquetaCode, pdfDocument);
                        barcodeImage.SetWidth(150);
                        barcodeImage.SetHeight(150);

                        document.Add(barcodeImage);

                        // Adiciona um espaço entre as etiquetas
                        document.Add(new Paragraph("\n"));
                    }
                }

                document.Close();
                return ms.ToArray();
            }
        }

        private PdfImage GerarCodigoDeBarras(string codigo, string tipo, PdfDocument pdfDocument)
        {
            switch (tipo.ToUpper())
            {
                case "EAN13":
                    if (codigo.Length != 12 || !long.TryParse(codigo, out _))
                    {
                        throw new ArgumentException("O código EAN-13 deve conter exatamente 12 dígitos numéricos.");
                    }
                    string codigoEAN13 = GerarCodigoEAN13(codigo);
                    var barcodeEAN = new BarcodeEAN(pdfDocument);
                    barcodeEAN.SetCodeType(BarcodeEAN.EAN13);
                    barcodeEAN.SetCode(codigoEAN13);
                    return new PdfImage(barcodeEAN.CreateFormXObject(null, null, pdfDocument));

                case "UPCA":
                    if (codigo.Length != 11 || !long.TryParse(codigo, out _))
                    {
                        throw new ArgumentException("O código UPC-A deve conter exatamente 11 dígitos numéricos.");
                    }
                    var barcodeUPCA = new BarcodeEAN(pdfDocument);
                    barcodeUPCA.SetCodeType(BarcodeEAN.UPCA);
                    barcodeUPCA.SetCode(codigo);
                    return new PdfImage(barcodeUPCA.CreateFormXObject(null, null, pdfDocument));

                case "C128A":
                    var barcode128A = new Barcode128(pdfDocument);
                    barcode128A.SetCodeType(Barcode128.CODE128);
                    barcode128A.SetCode(codigo);
                    return new PdfImage(barcode128A.CreateFormXObject(null, null, pdfDocument));

                case "CODABAR":
                    // Adicionar caracteres de início e fim, se necessário
                    if (!"ABCD".Contains(codigo[0]) || !"ABCD".Contains(codigo[^1]))
                    {
                        codigo = $"A{codigo}A"; // Prefixa e sufixa com 'A'
                    }
                    var barcodeCodabar = new BarcodeCodabar(pdfDocument);
                    barcodeCodabar.SetCode(codigo);
                    return new PdfImage(barcodeCodabar.CreateFormXObject(null, null, pdfDocument));

                case "QR":
                    var barcodeQR = new BarcodeQRCode(codigo);
                    return new PdfImage(barcodeQR.CreateFormXObject(pdfDocument));

                default:
                    throw new NotSupportedException($"Formato de código de barras '{tipo}' não é suportado.");
            }
        }

        private string GerarCodigoEAN13(string codigo)
        {
            if (codigo.Length != 12 || !long.TryParse(codigo, out _))
            {
                throw new ArgumentException("O código deve conter exatamente 12 dígitos numéricos.");
            }

            int somaPar = 0, somaImpar = 0;

            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(codigo[i].ToString());
                if (i % 2 == 0)
                {
                    somaImpar += digito;
                }
                else
                {
                    somaPar += digito;
                }
            }

            int somaTotal = somaImpar + (somaPar * 3);
            int digitoVerificador = (10 - (somaTotal % 10)) % 10;

            return codigo + digitoVerificador;
        }

        [HttpGet("relatorios/imprimir/{id}/{tipo}")]
        [Permissao("vOs", "Você não possui permissão para visualizar OS.")]
        public async Task<IActionResult> Imprimir(int id, string tipo = "a4")
        {
            var os = await _repositorioOs.ObterPorId(id);
            if (os == null)
            {
                TempData["Error"] = "Os não encontrada.";
                return RedirectToRoute("/os");
            }

            var emitente = await _repositorioEmitente.GetLastRegister();
            var produtos = _repositorioOs.GetProdutos(id);
            var servicos = _repositorioOs.GetServicos(id);
            var anotacoes = _repositorioOs.GetAnotacaoes(id);
            var podeGerarPix = ConfiguracaoHelper.ObterValor("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_ACCESS_TOKEN");
            var imprimirAnexo = ConfiguracaoHelper.ObterValor("IMPRIMIR_ANEXOS") ?? "false";
            var controlVias = ConfiguracaoHelper.ObterValor("control_2vias");
            var pixKey = ConfiguracaoHelper.ObterValor("pix_key");
            List<ArquivoModelOs> anexos = new List<ArquivoModelOs>();
            
            if (imprimirAnexo == "true")
            {
                // Defina o caminho onde os anexos dessa OS estão armazenados
                string pastaAnexos = Path.Combine("wwwroot/anexos", $"OS-{id}");
                if (Directory.Exists(pastaAnexos))
                {
                    var arquivos = Directory.GetFiles(pastaAnexos);
                    foreach (var arquivo in arquivos)
                    {
                        string fileName = Path.GetFileName(arquivo);
                        string fileUrl = $"/anexos/OS-{id}/{fileName}";

                        anexos.Add(new ArquivoModelOs
                        {
                            FileName = fileName,
                            Url = fileUrl,
                            FileType = ObterTipoArquivo(fileName)
                        });
                    }
                }
            }

            string qrCode = string.Empty;
            string qrCodeBase64 = string.Empty;
            string chaveFormatada = string.Empty;

            if (!string.IsNullOrEmpty(pixKey))
            {
                var pix = await RetornarPix("S", id, "os");
                if (pix != null && pix.Status == "success")
                {
                    qrCode = pix.QrCode ?? string.Empty;
                    qrCodeBase64 = pix.QrCodeBase64 ?? string.Empty;
                    chaveFormatada = pix.ChaveFormatada ?? string.Empty;
                }
            }

            var viewmodel = new OsEmailViewModel
            {
                Os = os,
                Produtos = produtos.ToList(),
                Servicos = servicos.ToList(),
                Anotacoes = anotacoes.ToList(),
                Emitente = emitente,
                PodeGerarPix = podeGerarPix,
                Anexos = anexos,
                Control2Vias = controlVias,
                QrCode = qrCode,
                QrCodeBase64 = qrCodeBase64,
                ChaveFormatada = chaveFormatada
            };
            
            string view = "Views/Relatorios/Os/papel-a4.cshtml";
            if (tipo == "termica")
                view = "Views/Relatorios/Os/papel-termica.cshtml";

            // Renderiza a view em HTML
            //var htmlContent = await this.RenderViewToStringAsync(view, viewmodel);

            // Gera o PDF a partir do HTML
            //var pdfBytes = await GerarPdf(htmlContent);

            // Retorna o PDF para o usuário
            //return File(pdfBytes, "application/pdf", $"OS-{id}.pdf");
            return View(view, viewmodel);
        }

        private async Task<byte[]> GerarPdf(string htmlContent)
        {
            // Baixa o navegador Chromium (somente na primeira execução)
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            // Inicia o navegador
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            using var page = await browser.NewPageAsync();

            // Define o conteúdo HTML da página
            await page.SetContentAsync(htmlContent);

            // Gera o PDF
            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20mm",
                    Right = "20mm",
                    Bottom = "20mm",
                    Left = "20mm"
                }
            });

            return pdfBytes;
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = this.HttpContext.RequestServices };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            var viewEngineResult = _viewEngine.FindView(actionContext, viewName, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"View '{viewName}' não encontrada.");
            }

            var view = viewEngineResult.View;
            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(httpContext, _tempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);
                return writer.ToString();
            }
        }

        private string ObterTipoArquivo(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
            {
                return "image";
            }
            return "file";
        }

        private async Task<PixResponse> RetornarPix(string estatico, int id, string tipo)
        {
            try
            {
                var apiUrl = $"{_configuration["ApiUrls:Pix"]}/gerar-pix-estatico/{id}?tipo={tipo}";
                if (estatico == "N")
                {
                    apiUrl = $"{_configuration["ApiUrls:Pix"]}/gerar/{id}?tipo={tipo}";
                }

                var response = await _httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    // Desserializa diretamente para PixResponse
                    return await response.Content.ReadFromJsonAsync<PixResponse>();
                }

                // Retorna um objeto PixResponse em caso de erro
                return new PixResponse
                {
                    Status = "error",
                    Message = "Falha ao retornar chave pix."
                };
            }
            catch (Exception ex)
            {
                // Retorna um objeto PixResponse em caso de exceção
                return new PixResponse
                {
                    Status = "error",
                    Message = ex.Message
                };
            }
        }

        [HttpGet("relatorios/imprimir-garantia/{id}/{tipo}")]
        [Permissao("vOs", "Você não possui permissão para visualizar OS.")]
        public async Task<IActionResult> ImprimirGarantia(int id, string tipo = "os")
        {
            var os = await _repositorioOs.ObterPorId(id);
            if (os == null)
            {
                TempData["Error"] = "Os não encontrada.";
                return RedirectToRoute("/os");
            }

            var emitente = await _repositorioEmitente.GetLastRegister();
            var viewmodel = new OsEmailViewModel
            {
                Os = os,
                Emitente = emitente
            };

            string view = "Views/Relatorios/Garantias/garantia-os.cshtml";
            return View(view, viewmodel);
        }
    }
}
