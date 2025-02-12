using SIGES.Models.Requests;
using SIGES.Models;
using SIGES.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace SIGES.Interfaces
{
    public interface IPaymentGateway
    {
        Task<object> CriarCobranca(PaymentRequest request);
        Task<bool> CancelarCobranca(int id);
        Task<(bool confirm, string msg)> ConfirmarPagamento(int id);
        Task<bool> AtualizarStatus(int id);
        Task<PixResponse> GerarPix(PaymentRequest request);
    }
}
