using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface ICategoriaRepositorio
    {
        Task<int> GetMaxId();

        Task Adicionar(CategoriaModel categoria);

        IQueryable<CategoriaModel> Listar();

        Task<CategoriaModel?> ObterPorId(int id);

        Task Atualizar(CategoriaModel categoria);

        Task Excluir(int id);
    }
}