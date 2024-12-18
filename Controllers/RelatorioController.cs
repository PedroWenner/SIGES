using iText.Barcodes;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using System.Drawing;
using DrawingImage = System.Drawing.Image;
using PdfImage = iText.Layout.Element.Image;
using ZXing;
using ZXing.Common;

namespace SIGES.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        public RelatorioController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
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

                //case "C93":
                //    var barcode93Writer = new BarcodeWriter<Bitmap>
                //    {
                //        Format = BarcodeFormat.CODE_93,
                //        Options = new EncodingOptions { Width = 300, Height = 150 }
                //    };
                //    var bitmap93 = barcode93Writer.Write(codigo);
                //    using (var stream = new MemoryStream())
                //    {
                //        bitmap93.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                //        return new PdfImage(ImageDataFactory.Create(stream.ToArray()));
                //    }

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
    }
}
