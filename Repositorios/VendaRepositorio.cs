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
            return await _context.Vendas
                .Include(c => c.Cliente)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);
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
        public async Task<EmitenteModel> ObterEmitente()
        {
            return await _context.Emitente.FirstOrDefaultAsync();
        }
        public IEnumerable<ItensVendaModel?> GetProdutos(int id)
        {
            return _context.ItensVenda
                .Include(c => c.Produto)
                .Include(p => p.Venda)
                .Where(c => c.VendaId == id)
                .ToList();
        }
        /// <summary>
        /// Retorna todas as cobranças não aprovadas vinculadas a uma venda
        /// </summary>
        /// <param name="id">ID da venda (venda_id).</param>
        /// <returns></returns>
        public IEnumerable<Cobranca?> GetCobrancas(int id)
        {
            return _context.Cobrancas
                .Where(c => c.VendasId == id)
                .ToList();
        }
        /// <summary>
        /// Exclui todos os produtos vinculados a uma venda.
        /// </summary>
        /// <param name="id">ID da venda (venda_id).</param>
        /// <returns></returns>
        public async Task ExcluirProdutos(int id)
        {
            var produtos = await _context.ItensVenda
                .Include(i => i.Produto)
                .Where(c => c.VendaId == id)
                .ToListAsync();

            if (produtos.Any())
            {
                // Atualiza o estoque de cada produto
                foreach (var p in produtos)
                {
                    p.Produto.Estoque += p.Quantidade;
                }

                _context.ItensVenda.RemoveRange(produtos);
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Exclui o lancamento vinculados a uma venda.
        /// </summary>
        /// <param name="id">ID da venda (venda_id).</param>
        /// <returns></returns>
        public async Task ExcluirLancamento(int id)
        {
            var lancamento = await _context.Lancamentos
                .Where(c => c.VendasId == id)
                .ToListAsync();

            if (lancamento.Any())
            {
                _context.Lancamentos.RemoveRange(lancamento);
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Exclui as cobranças vinculadas a uma venda.
        /// </summary>
        /// <param name="id">ID da venda (venda_id).</param>
        /// <returns></returns>
        public async Task ExcluirCobrancas(int id)
        {
            var cobrancas = await _context.Cobrancas
                .Where(c => c.VendasId == id)
                .ToListAsync();

            if (cobrancas.Any())
            {
                _context.Cobrancas.RemoveRange(cobrancas);
                await _context.SaveChangesAsync();
            }
        }
    }
}