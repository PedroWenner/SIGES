using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetLastRegister();
        Task Adicionar(T entidade);
        Task Atualizar(T entidade);
        Task Deletar(T entidade);
        Task<T?> ObterPorId(int id);
        Task<IEnumerable<T>> Listar();
    }
}
