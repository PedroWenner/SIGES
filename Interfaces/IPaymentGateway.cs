using SIGES.Models.Requests;
using SIGES.Models;
using SIGES.ViewModel;

namespace SIGES.Interfaces
{
    public interface IPaymentGateway
    {
        Task<object> CriarCobranca(PaymentRequest request);
        Task<bool> CancelarCobranca(int id);
        Task<bool> ConfirmarPagamento(int id);
    }
}
