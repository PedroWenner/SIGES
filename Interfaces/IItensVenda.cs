using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IItensVenda
    {
        Task Adicionar(ItensVendaModel item);

        IEnumerable<ItensVendaModel> Listar();

        Task<ItensVendaModel?> ObterPorId(int id);

        Task Atualizar(ItensVendaModel item);

        Task Excluir(int id);
    }
}