using Microsoft.EntityFrameworkCore;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Repositorios
{
    public class OsRepositorio : IOs
    {
        private readonly AppDbContext _context;
        public OsRepositorio(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<OsModel> Listar()
        {
            return _context.Os
                .Include(o => o.Usuarios)
                .Include(o => o.Clientes)
                .Include(o => o.Servicos)
                    .ThenInclude(s => s.Servicos)
                .Include(o => o.Garantias)
                .ToList();
        }
        public async Task<OsModel?> ObterPorId(int id)
        {
            return await _context.Os
                .Include(o => o.Usuarios)
                .Include(o => o.Clientes)
                .Include(o => o.Servicos)
                    .ThenInclude(s => s.Servicos)
                .Include(o => o.Garantias)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task Adicionar(OsModel os)
        {
            _context.Os.Add(os);
            await _context.SaveChangesAsync();
        }
        public async Task Atualizar(OsModel os)
        {
            _context.Os.Update(os);
            await _context.SaveChangesAsync();
        }
        public async Task Excluir(int id)
        {
            var os = await _context.Os.FirstOrDefaultAsync(c => c.Id == id);

            if (os != null)
            {
                _context.Os.Remove(os);
                await _context.SaveChangesAsync();
            }
        }
        public IEnumerable<ServicosOs?> GetServicos(int id)
        {
            return _context.ServicosOs
                .Include(s => s.Servicos)
                .Where(s => s.OsId == id)
                .ToList();
        }
        public IEnumerable<ProdutosOs?> GetProdutos(int id)
        {
            return _context.ProdutosOs
                .Include(s => s.Produtos)
                .Where(s => s.OsId == id)
                .ToList();
        }
    }
}
