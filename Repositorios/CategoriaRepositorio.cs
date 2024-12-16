using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly AppDbContext _context;

        public CategoriaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetMaxId()
        {
            return await _context.Categorias.MaxAsync(c => (int?)c.Id) ?? 0;
        }

        public IEnumerable<CategoriaModel> Listar()
        {
            return _context.Categorias
                .ToList();
        }

        public async Task<CategoriaModel?> ObterPorId(int id)
        {
            return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(CategoriaModel categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(CategoriaModel categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}