using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly AppDbContext _context;

        public PedidoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PedidoModel>> ListarTodos()
        {
            return await _context.Pedidos
                .Include(p => p.Produto)
                .Include(p => p.Cliente)
                .ToListAsync();
        }
        public async Task Adicionar(PedidoModel pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
        }
    }
}