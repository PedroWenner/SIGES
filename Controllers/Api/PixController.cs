using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models.Requests;
using pix_payload_generator.net.Models.CobrancaModels;
using pix_payload_generator.net.Models.PayloadModels;
using QRCoder;
using SIGES.Models;
using Cobranca = pix_payload_generator.net.Models.CobrancaModels.Cobranca;
using System.Globalization;
using System.Text;
using SIGES.Helpers;

[Route("api/pix")]
[ApiController]
public class PixController : ControllerBase
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOs _osRepositorio;
    private readonly IRepository<EmitenteModel> _repositorioEmitente;

    public PixController(IPaymentGateway paymentGateway, IOs osRepositorio, IRepository<EmitenteModel> repositorioEmitente)
    {
        _paymentGateway = paymentGateway;
        _osRepositorio = osRepositorio;
        _repositorioEmitente = repositorioEmitente;
    }

    // Chama a API para gerar o Pix e retorna o QR Code
    [HttpPost("gerar/{id}")]
    public async Task<PixResponse> GerarPix(int id, string tipo = "os")
    {
        try
        {
            if (id == 0)
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = $"Problemas no envio do {id}",
                    Acao = "error"
                };
            }

            var model = await _osRepositorio.ObterPorId(id);
            var emitente = await _repositorioEmitente.GetLastRegister();

            if (model == null)
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = "OS/Venda não encontrado.",
                    Acao = "error"
                };
            }
            if (emitente == null)
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = "Emitente não confirado.",
                    Acao = "error"
                };
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
                if (pix != null && pix.Status == "success")
                {
                    qrCode = pix.QrCode ?? string.Empty;
                    qrCodeBase64 = pix.QrCodeBase64 ?? string.Empty;
                }

                return new PixResponse
                {
                    Status = "success",
                    Message = "Pix gerada com sucesso!",
                    Acao = "success",
                    QrCode = qrCode,
                    QrCodeBase64 = qrCodeBase64
                };
            }

            return new PixResponse
            {
                Status = "error",
                Message = "Não é possível gerar pix de OS/Venda sem valor.",
                Acao = "error"
            };

        }
        catch (Exception ex)
        {
            return new PixResponse
            {
                Status = "error",
                Message = ex.Message,
                Acao = "error"
            };
        }
    }

    // Chama a API para gerar o Pix estático e retorna o QR Code
    [HttpPost("gerar-pix-estatico/{id}")]
    public async Task<PixResponse> GerarPixEstatico(int id, string tipo = "os")
    {
        try
        {
            if (id == 0)
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = $"Problemas no envio do {id}",
                    Acao = "error"
                };
            }

            var model = await _osRepositorio.ObterPorId(id);
            var emitente = await _repositorioEmitente.GetLastRegister();
            var pixKey = ConfiguracaoHelper.ObterValor("pix_key");
            
            if (model == null)
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = "OS/Venda não encontrado.",
                    Acao = "error"
                };
            }
            if (emitente == null)
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = "Emitente não confirado.",
                    Acao = "error"
                };
            }
            if (string.IsNullOrEmpty(pixKey))
            {
                return new PixResponse
                {
                    Status = "error",
                    Message = "Chave pix não configurada.",
                    Acao = "error"
                };
            }

            string cidadeEmitente = RemoverAcentos(emitente.Cidade.ToUpper());
            string nomeEmitente = RemoverAcentos(emitente.Nome.ToUpper());
            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);
            decimal? valorTotal = produtos.Sum(i => i.SubTotal) + servicos.Sum(i => i.SubTotal ?? 0);

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

                string chaveFormatada = FormatarChavePix(pixKey);
                return new PixResponse
                {
                    Status = "success",
                    Message = "Pix gerada com sucesso!",
                    Acao = "success",
                    QrCode = stringToQrCode,
                    QrCodeBase64 = qrCodeImg,
                    ChaveFormatada = chaveFormatada
                };
            }

            return new PixResponse
            {
                Status = "error",
                Message = "Não é possível gerar pix de OS/Venda sem valor.",
                Acao = "error"
            };
        }
        catch (Exception ex)
        {
            return new PixResponse
            {
                Status = "error",
                Message = ex.Message,
                Acao = "error"
            };
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

    public string FormatarChavePix(string chave)
    {
        if (chave.Length == 11) // CPF
            return Convert.ToUInt64(chave).ToString(@"000\.000\.000\-00");
        else if (chave.Length == 14) // CNPJ
            return Convert.ToUInt64(chave).ToString(@"00\.000\.000\/0000\-00");
        else if (chave.Length == 32)
            return chave.Insert(8, "-");
        return chave;
    }
}

public class PixResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public string Acao { get; set; }
    public string QrCode { get; set; }
    public string QrCodeBase64 { get; set; }
    public string ChaveFormatada { get; set; }
}