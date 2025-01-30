using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;
using SIGES.Models.Requests;
using SIGES.Migrations;

namespace SIGES.Repositorios
{
    public class CobrancaRepository : ICobranca
    {
        private readonly AppDbContext _context;

        public CobrancaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> ObterCobrancaPorId(int id)
        {
            return await _context.Cobrancas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cobranca>> Listar()
        {
            return await _context.Cobrancas
                .Include(c => c.Cliente)
                .Include(c => c.Venda)
                .Include(c => c.Os)
                .ToListAsync();
        }

        public async Task ExcluirCobranca(int id)
        {
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            _context.Cobrancas.Remove(cobranca);
            await _context.SaveChangesAsync();
        }
    }
}