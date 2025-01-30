using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IVenda
    {
        Task Adicionar(VendaModel venda);
        IEnumerable<VendaModel> Listar();
        Task<VendaModel?> ObterPorId(int id);
        Task Atualizar(VendaModel venda);
        Task Excluir(int id);
        Task<EmitenteModel> ObterEmitente();
        IEnumerable<ItensVendaModel?> GetProdutos(int id);
    }
}