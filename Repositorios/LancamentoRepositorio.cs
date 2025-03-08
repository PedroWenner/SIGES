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

        public async Task<IEnumerable<LancamentoModel>> GetVendasOs()
        {
            var query = from l in _context.Lancamentos
                        join v in _context.Vendas on l.VendasId equals v.Id into vendaGroup
                        from v in vendaGroup.DefaultIfEmpty()
                        join o in _context.Os on l.OsId equals o.Id into osGroup
                        from o in osGroup.DefaultIfEmpty()
                        join c in _context.Clientes on l.ClientesId equals c.Id
                        select new LancamentoModel
                        {
                            Id = l.Id,
                            Tipo = l.Tipo,
                            ClientesId = l.ClientesId,
                            Descricao = l.Descricao,
                            DataVencimento = l.DataVencimento,
                            Baixado = l.Baixado,
                            Observacoes = l.Observacoes,
                            FormaPgto = l.FormaPgto,
                            Valor = l.Valor,
                            Desconto = l.Desconto,
                            ValorDesconto = l.ValorDesconto,
                            Clientes = c
                        };

            return await query.ToListAsync();
        }
    }
}