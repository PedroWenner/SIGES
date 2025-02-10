using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models.Requests;

[Route("api/pix")]
[ApiController]
public class PixController : ControllerBase
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOs _osRepositorio;

    public PixController(IPaymentGateway paymentGateway, IOs osRepositorio)
    {
        _paymentGateway = paymentGateway;
        _osRepositorio = osRepositorio;
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
}