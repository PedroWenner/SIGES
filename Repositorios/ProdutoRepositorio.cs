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

        public async Task<int> GetMaxCodigo()
        {
            return await _context.Produtos.MaxAsync(c => (int?)c.Codigo) ?? 0;
        }

        public async Task<IList<ProdutoModel>> Listar()
        {
            return await _context.Produtos
                .Include(p => p.Categoria) // Faz o join com a tabela de categorias
                .ToListAsync();
        }

        public async Task Adicionar(ProdutoModel produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdutoModel> ObterProdutoPorId(int? id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task Atualizar(ProdutoModel produto)
        {
            var produtoExistente = await _context.Produtos.FindAsync(produto.Id);

            if (produtoExistente != null)
            {
                produtoExistente.Codigo = produto.Codigo;
                produtoExistente.Status = produto.Status;
                produtoExistente.Descricao = produto.Descricao;
                produtoExistente.Peso = produto.Peso;
                produtoExistente.UnidadeMedida = produto.UnidadeMedida;
                produtoExistente.CategoriaId = produto.CategoriaId;
                produtoExistente.Valor = produto.Valor;
                produtoExistente.Observacao = produto.Observacao;
                produtoExistente.CaminhoImagem = produto.CaminhoImagem;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProdutoModel> Excluir(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }

            return produto;
        }

        public async Task<IList<ProdutoModel>> ObterProdutosPorCategoriaId(int categoriaId)
        {
            return await _context.Produtos
                .Where(p => p.CategoriaId == categoriaId)
                .ToListAsync();
        }
    }
}