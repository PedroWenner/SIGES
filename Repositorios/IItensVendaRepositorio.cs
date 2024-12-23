using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class IItensVendaRepositorio : IItensVenda
    {
        private readonly AppDbContext _context;

        public IItensVendaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ItensVendaModel> Listar()
        {
            return _context.ItensVenda
                .Include(c => c.Produto)
                .Include(p => p.Venda)
                .ToList();
        }

        public async Task<ItensVendaModel?> ObterPorId(int id)
        {
            return await _context.ItensVenda
                .Include(c => c.Produto)
                .Include(p => p.Venda)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(ItensVendaModel item)
        {
            _context.ItensVenda.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(ItensVendaModel item)
        {
            _context.ItensVenda.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var item = await _context.ItensVenda.Include(i => i.Produto).FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                // Repor a quantidade no estoque do produto
                item.Produto.Estoque += item.Quantidade ?? 0;

                _context.ItensVenda.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}