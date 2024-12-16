using Microsoft.EntityFrameworkCore;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Repositorios
{
    public class ServicoRepositorio : IServico
    {
        private readonly AppDbContext _context;

        public DbSet<ServicoModel> Clientes { get; set; }

        public ServicoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ServicoModel> Listar()
        {
            return _context.Servicos
                .ToList();
        }
        public async Task<ServicoModel?> ObterPorId(int id)
        {
            return await _context.Servicos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(ServicoModel servico)
        {
            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(ServicoModel servico)
        {
            _context.Servicos.Update(servico);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var servico = await _context.Servicos.FirstOrDefaultAsync(c => c.Id == id);

            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
            }
        }
    }
}
