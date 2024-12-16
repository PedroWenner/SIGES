using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SIGES.Repositorios
{
    public class PermissaoRepositorio : IPermissaoRepositorio
    {
        private readonly AppDbContext _context;

        public PermissaoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PermissaoModel> Listar()
        {
            return _context.Permissoes.ToList();
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

        // Métodos para gerenciar permissões do usuário
        public async Task<List<PermissaoDetalheModel>> ObterPermissoesUsuario(int userId)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Permissao)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario?.Permissao?.Permissoes == null)
                return new List<PermissaoDetalheModel>();

            // Desserializar o JSON para uma lista de PermissaoDetalheModel
            return JsonConvert.DeserializeObject<List<PermissaoDetalheModel>>(usuario.Permissao.Permissoes) ?? new List<PermissaoDetalheModel>();
        }

        public async Task<bool> VerificarPermissao(int userId, string permissao)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Permissao)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return usuario != null && usuario.Permissao != null && usuario.Permissao.Nome == permissao;
        }
    }
}