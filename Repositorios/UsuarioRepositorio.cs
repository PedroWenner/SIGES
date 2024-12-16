using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext _context;

        public UsuarioRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UsuarioModel> Listar()
        {
            return _context.Usuarios
                .Include(x => x.Permissao)
                .ToList();
        }

        public async Task<UsuarioModel?> ObterPorId(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(UsuarioModel usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(UsuarioModel usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
