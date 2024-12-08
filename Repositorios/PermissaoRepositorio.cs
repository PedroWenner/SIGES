using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class PermissaoRepositorio : IPermissaoRepositorio
    {
        private readonly AppDbContext _context;

        public PermissaoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<PermissaoModel> Listar()
        {
            return _context.Permissoes
                .AsQueryable();
        }

        public async Task<PermissaoModel?> ObterPorId(int id)
        {
            return await _context.Permissoes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(PermissaoModel permissao)
        {
            _context.Permissoes.Add(permissao);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(PermissaoModel permissao)
        {
            _context.Permissoes.Update(permissao);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var permissao = await _context.Permissoes.FirstOrDefaultAsync(c => c.Id == id);

            if (permissao != null)
            {
                _context.Permissoes.Remove(permissao);
                await _context.SaveChangesAsync();
            }
        }
    }
}