using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models.Requests;
using pix_payload_generator.net.Models.CobrancaModels;
using pix_payload_generator.net.Models.PayloadModels;
using QRCoder;
using SIGES.Models;
using Cobranca = pix_payload_generator.net.Models.CobrancaModels.Cobranca;
using System.Security.Cryptography.Xml;
using ZXing.QrCode.Internal;
using System.Globalization;
using System.Text;

[Route("api/pix")]
[ApiController]
public class PixController : ControllerBase
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOs _osRepositorio;
    private readonly IConfiguracao _configuracaoRepositorio;
    private readonly IRepository<EmitenteModel> _repositorioEmitente;

    public PixController(IPaymentGateway paymentGateway, IOs osRepositorio, IConfiguracao configuracaoRepositorio, IRepository<EmitenteModel> repositorioEmitente)
    {
        _paymentGateway = paymentGateway;
        _osRepositorio = osRepositorio;
        _configuracaoRepositorio = configuracaoRepositorio;
        _repositorioEmitente = repositorioEmitente;
    }

    // Chama a API para gerar o Pix e retorna o QR Code
    [HttpPost("gerar/{id}")]
    public async Task<object> GerarPix(int id, string tipo = "os")
    {
        try
        {
            if (id == 0)
            {
                return new { status = "error", message = $"Problemas no envio do {id}", acao = "error" };
            }

            var model = await _osRepositorio.ObterPorId(id);
            if (model == null)
            {
                return new { status = "error", message = "OS/Venda não encontrado.", acao = "error" };
            }

            string qrCode = string.Empty;
            string qrCodeBase64 = string.Empty;
            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);

            decimal? valorTotal = produtos.Sum(i => i.SubTotal) + servicos.Sum(i => i.SubTotal ?? 0);

            if (valorTotal > 0)
            {
                PaymentRequest request = new PaymentRequest
                {
                    Id = model.Id,
                    Tipo = "os",
                    TransactionAmount = valorTotal,
                    Description = $"OS {id}",
                    PaymentMethodId = "pix",
                    Email = model.Clientes?.Email ?? "",
                    Identification = model.Clientes?.Documento?.Replace(".", "").Replace("-", "") ?? ""
                };

                var pix = await _paymentGateway.GerarPix(request);
                if (pix is not null)
                {
                    qrCode = pix.GetType().GetProperty("qrCode")?.GetValue(pix)?.ToString() ?? string.Empty;
                    qrCodeBase64 = pix.GetType().GetProperty("qrCodeBase64")?.GetValue(pix)?.ToString() ?? string.Empty;
                }

                return pix;
            }

            return new { status = "error", message = "Não é possível gerar pix de OS/Venda sem valor.", acao = "error" };

        }
        catch (Exception ex)
        {
            return new { status = "error", message = ex.Message, acao = "error" };
        }
    }

    // Chama a API para gerar o Pix estático e retorna o QR Code
    [HttpPost("gerar-pix-estatico/{id}")]
    public async Task<object> GerarPixEstatico(int id, string tipo = "os")
    {
        try
        {
            if (id == 0)
            {
                return new { status = "error", message = $"Problemas no envio do {id}", acao = "error" };
            }

            var model = await _osRepositorio.ObterPorId(id);
            if (model == null)
            {
                return new { status = "error", message = "OS/Venda não encontrado.", acao = "error" };
            }

            var emitente = await _repositorioEmitente.GetLastRegister();
            if (emitente == null)
            {
                return new { status = "error", message = "Emitente não confirado.", acao = "error" };
            }

            string cidadeEmitente = RemoverAcentos(emitente.Cidade.ToUpper());
            string nomeEmitente = RemoverAcentos(emitente.Nome.ToUpper());
            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);
            var pixKey = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("pix_key");

            decimal? valorTotal = produtos.Sum(i => i.SubTotal) + servicos.Sum(i => i.SubTotal ?? 0);

            if (string.IsNullOrEmpty(pixKey))
            {
                return new { status = "error", message = "Chave pix não configuradao.", acao = "error" };
            }

            if (valorTotal > 0 && !string.IsNullOrEmpty(pixKey))
            {
                Cobranca cobranca = new Cobranca(_chave: pixKey)
                {
                    SolicitacaoPagador = $"OS {id}",
                    Valor = new Valor
                    {
                        Original = valorTotal?.ToString("0.00", CultureInfo.InvariantCulture)
                    }
                };

                var payload = cobranca.ToPayload(GerarTxid(), new Merchant(nomeEmitente, cidadeEmitente));
                var stringToQrCode = payload.GenerateStringToQrCode();
                string qrCodeImg = string.Empty;
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(stringToQrCode, QRCodeGenerator.ECCLevel.Q);
                    var qrCode = new PngByteQRCode(qrCodeData);

                    qrCodeImg = $"data:image/png;base64,{Convert.ToBase64String(qrCode.GetGraphic(20))}";
                }

                return new
                {
                    status = "success",
                    message = "Pix gerada com sucesso!",
                    acao = "success",
                    qrCode = stringToQrCode,
                    qrCodeBase64 = qrCodeImg
                };
            }

            return new { status = "error", message = "Não é possível gerar pix de OS/Venda sem valor.", acao = "error" };

        }
        catch (Exception ex)
        {
            return new { status = "error", message = ex.Message, acao = "error" };
        }
    }

    public string GerarTxid()
    {
        return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 25);
    }

    public string RemoverAcentos(string texto)
    {
        if (string.IsNullOrEmpty(texto)) return texto;
        return new string(texto.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
    }
}