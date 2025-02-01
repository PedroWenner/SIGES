using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class EmitenteRepositorio : IEmitente
    {
        private readonly AppDbContext _context;

        public EmitenteRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EmitenteModel> Listar()
        {
            return _context.Emitente
                .ToList();
        }

        public async Task<EmitenteModel?> ObterPorId(int id)
        {
            return await _context.Emitente
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(EmitenteModel emitente)
        {
            _context.Emitente.Add(emitente);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(EmitenteModel emitente)
        {
            _context.Emitente.Update(emitente);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var emitente = await _context.Emitente
                .FirstOrDefaultAsync(c => c.Id == id);

            if (emitente != null)
            {
                _context.Emitente.Remove(emitente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EmitenteModel?> ObterEmitente()
        {
            return await _context.Emitente
                .FirstOrDefaultAsync();
        }
    }
}