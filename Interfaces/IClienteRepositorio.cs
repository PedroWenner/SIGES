using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IClienteRepositorio
    {
        Task<List<ClienteModel>> ListarTodos();
        
        Task Adicionar(ClienteModel cliente);

        Task<ClienteModel> ObterClientePorId(int? id);

        Task Atualizar(ClienteModel cliente);

        Task<ClienteModel> Excluir(int id);
    }
}
