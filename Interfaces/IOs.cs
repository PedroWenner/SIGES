using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IOs
    {
        IEnumerable<OsModel> Listar();
        Task<OsModel?> ObterPorId(int id);
        Task Adicionar(OsModel os);
        Task Atualizar(OsModel os);
        Task Excluir(int id);
        IEnumerable<ServicosOs?> GetServicos(int id);
        IEnumerable<ProdutosOs?> GetProdutos(int id);
        IEnumerable<AnotacoesOs?> GetAnotacaoes(int id);
    }
}
