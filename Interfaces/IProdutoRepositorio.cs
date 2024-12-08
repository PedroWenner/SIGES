using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IProdutoRepositorio
    {
        Task<int> GetMaxCodigo();

        Task<IList<ProdutoModel>> Listar();

        Task<ProdutoModel> ObterProdutoPorId(int? id);

        Task Adicionar(ProdutoModel produto);

        Task Atualizar(ProdutoModel produto);

        Task<ProdutoModel> Excluir(int id);

        Task<IList<ProdutoModel>> ObterProdutosPorCategoriaId(int categoriaId);
    }
}
