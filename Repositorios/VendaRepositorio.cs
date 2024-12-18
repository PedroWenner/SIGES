using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class VendaRepositorio : IVenda
    {
        private readonly AppDbContext _context;

        public VendaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<VendaModel> Listar()
        {
            return _context.Vendas
                .Include(c => c.Cliente)
                .Include(p => p.Usuario)
                .ToList();
        }

        public async Task<VendaModel?> ObterPorId(int id)
        {
            return await _context.Vendas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(VendaModel venda)
        {
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(VendaModel venda)
        {
            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var venda = await _context.Vendas.FirstOrDefaultAsync(c => c.Id == id);

            if (venda != null)
            {
                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
            }
        }
    }
}