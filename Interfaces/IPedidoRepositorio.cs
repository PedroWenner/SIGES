using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IPedidoRepositorio
    {
        Task<List<PedidoModel>> ListarTodos();
        Task Adicionar(PedidoModel pedido);
    }
}