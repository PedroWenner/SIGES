using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IEmitente
    {
        Task Adicionar(EmitenteModel emitente);

        IEnumerable<EmitenteModel> Listar();

        Task<EmitenteModel?> ObterPorId(int id);

        Task Atualizar(EmitenteModel emitente);

        Task Excluir(int id);
    }
}