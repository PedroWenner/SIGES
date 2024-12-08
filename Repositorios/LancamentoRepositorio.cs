using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class LancamentoRepositorio : ILancamentoRepositorio
    {
        private readonly AppDbContext _context;

        public LancamentoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(LancamentoModel lancamento)
        {
            _context.Lancamentos.Add(lancamento);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<LancamentoModel>> Listar()
        {
            var lancamentos = await _context.Lancamentos
                .Include(l => l.Categoria)
                .ToListAsync();

            return lancamentos;
        }

        public async Task<LancamentoModel> Excluir(int id)
        {
            var lancamento = await _context.Lancamentos.FindAsync(id);
            if (lancamento != null)
            {
                _context.Lancamentos.Remove(lancamento);
                await _context.SaveChangesAsync();
            }
            return lancamento;
        }

        public async Task Atualizar(LancamentoModel lancamento)
        {
            var existe = await _context.Lancamentos.FindAsync(lancamento.Id);
            if (existe != null)
            {
                existe.Descricao = lancamento.Descricao;
                _context.Lancamentos.Update(existe);
                await _context.SaveChangesAsync();
            }
        }
    }
}