using SIGES.Models;
using SIGES.Models.Requests;
using System.Threading.Tasks;

namespace SIGES.Interfaces
{
    public interface ICobranca
    {
        Task<object> ObterCobrancaPorId(int id);
        Task<IEnumerable<Cobranca>> Listar();
        Task ExcluirCobranca(int id);
    }
}
