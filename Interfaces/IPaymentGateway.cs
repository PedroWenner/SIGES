using SIGES.Models.Requests;
using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IPaymentGateway
    {
        Task<object> CriarCobranca(CobrancaRequest request);
        Task<bool> CancelarCobranca(int id);
        Task<bool> ConfirmarPagamento(int id);
    }
}
