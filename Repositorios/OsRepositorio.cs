using Microsoft.EntityFrameworkCore;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Repositorios
{
    public class OsRepositorio : IOs
    {
        private readonly AppDbContext _context;

        public DbSet<OsModel> Clientes { get; set; }

        public OsRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OsModel> Listar()
        {
            return _context.Ordens
                .Include(u => u.Usuario)
                .Include(c => c.Cliente)
                .ToList();
        }
        public async Task<OsModel?> ObterPorId(int id)
        {
            return await _context.Ordens
                .Include(u => u.Usuario)
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(OsModel os)
        {
            _context.Ordens.Add(os);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(OsModel os)
        {
            _context.Ordens.Update(os);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var os = await _context.Ordens.FirstOrDefaultAsync(c => c.Id == id);

            if (os != null)
            {
                _context.Ordens.Remove(os);
                await _context.SaveChangesAsync();
            }
        }
    }
}
