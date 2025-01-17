using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IGarantia
    {
        Task Adicionar(GarantiaModel garantia);
        IEnumerable<GarantiaModel> Listar();
        Task<GarantiaModel?> ObterPorId(int id);
        Task Atualizar(GarantiaModel garantia);
        Task Excluir(int id);
        Task<EmitenteModel> ObterEmitente();
    }
}