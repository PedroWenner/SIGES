using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly AppDbContext _context;

        public ProdutoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProdutoModel> Listar()
        {
            return _context.Produtos
                .ToList();
        }

        public async Task<ProdutoModel?> ObterPorId(int id)
        {
            return await _context.Produtos
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(ProdutoModel produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(ProdutoModel produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(c => c.Id == id);

            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProdutoModel>> ObterPorIntervalo(int deId, int ateId)
        {
            return await _context.Produtos
                .Where(p => p.Id >= deId && p.Id <= ateId)
                .ToListAsync();
        }
    }
}