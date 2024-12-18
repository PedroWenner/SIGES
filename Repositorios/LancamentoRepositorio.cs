using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class LancamentoRepositorio : ILancamento
    {
        private readonly AppDbContext _context;

        public LancamentoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LancamentoModel> Listar()
        {
            return _context.Lancamentos
                .ToList();
        }

        public async Task<LancamentoModel?> ObterPorId(int id)
        {
            return await _context.Lancamentos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(LancamentoModel lancamento)
        {
            _context.Lancamentos.Add(lancamento);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(LancamentoModel lancamento)
        {
            _context.Lancamentos.Update(lancamento);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var lancamento = await _context.Lancamentos.FirstOrDefaultAsync(c => c.Id == id);

            if (lancamento != null)
            {
                _context.Lancamentos.Remove(lancamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}