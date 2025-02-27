using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly AppDbContext _context;

        public DbSet<ClienteModel> Clientes { get; set; }

        public ClienteRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ClienteModel> Listar()
        {
            return _context.Clientes.ToList();
        }
        public async Task<ClienteModel?> ObterPorId(int id)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Adicionar(ClienteModel cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(ClienteModel cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> DocumentoExiste(string documento)
        {
            return await _context.Clientes.AnyAsync(u => u.Documento == documento);
        }

        public IEnumerable<OsModel?> GetOs(int id)
        {
            return _context.Os
                .Include(s => s.Clientes)
                .Where(s => s.ClientesId == id)
                .ToList();
        }

        public IEnumerable<VendaModel?> GetVendas(int id)
        {
            return _context.Vendas
                .Include(s => s.Cliente)
                .Where(s => s.ClienteId == id)
                .ToList();
        }
    }
}