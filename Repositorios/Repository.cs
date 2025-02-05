using Microsoft.EntityFrameworkCore;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetLastRegister()
        {
            return await _dbSet.OrderByDescending(e => EF.Property<int>(e, "Id")).FirstOrDefaultAsync();
        }

        public async Task Adicionar(T entidade)
        {
            await _dbSet.AddAsync(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(T entidade)
        {
            _dbSet.Update(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(T entidade)
        {
            _dbSet.Remove(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> ObterPorId(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> Listar()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
