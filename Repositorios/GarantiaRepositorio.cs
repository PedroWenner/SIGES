using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class GarantiaRepositorio : IGarantia
    {
        private readonly AppDbContext _context;

        public GarantiaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GarantiaModel> Listar()
        {
            return _context.Garantias
                .ToList();
        }

        public async Task<GarantiaModel?> ObterPorId(int id)
        {
            return await _context.Garantias
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(GarantiaModel garantia)
        {
            _context.Garantias.Add(garantia);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(GarantiaModel garantia)
        {
            _context.Garantias.Update(garantia);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var garantia = await _context.Garantias
                .FirstOrDefaultAsync(c => c.Id == id);

            if (garantia != null)
            {
                _context.Garantias.Remove(garantia);
                await _context.SaveChangesAsync();
            }
        }
    }
}