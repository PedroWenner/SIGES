using SIGES.Models;
using SIGES.Models.Requests;

namespace SIGES.Interfaces
{
    public interface ICobranca
    {
        Task<object> ObterCobrancaPorId(int id);
        Task<IEnumerable<Cobranca>> ObterTodasCobrancas();
    }
}
