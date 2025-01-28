using SIGES.Models;
using SIGES.Models.Requests;

namespace SIGES.Interfaces
{
    public interface ICobranca
    {
        Task<object> GerarCobranca(CobrancaRequest request);
        Task ExcluirCobranca(int id);
        Task AtualizarCobranca(int id);
        Task ConfirmarPagamento(int id);
        Task CancelarCobranca(int id);
        Task<object> ObterCobrancaPorId(int id);
        Task<IEnumerable<Cobranca>> ObterTodasCobrancas();
    }
}
