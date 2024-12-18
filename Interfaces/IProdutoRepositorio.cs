using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IProdutoRepositorio
    {
        IEnumerable<ProdutoModel> Listar();

        Task<ProdutoModel?> ObterPorId(int id);

        Task Adicionar(ProdutoModel produto);

        Task Atualizar(ProdutoModel produto);

        Task Excluir(int id);

        Task<List<ProdutoModel>> ObterPorIntervalo(int deId, int ateId);
    }
}
