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

        public async Task<List<ClienteModel>> ListarTodos()
        {
            return await _context.Clientes
                .ToListAsync();
        }

        public async Task Adicionar(ClienteModel cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<ClienteModel> ObterClientePorId(int? id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task Atualizar(ClienteModel cliente)
        {
            var clienteExistente = await _context.Clientes.FindAsync(cliente.Id);

            if (clienteExistente != null)
            {
                clienteExistente.Nome = cliente.Nome;
                clienteExistente.Documento = cliente.Documento;
                clienteExistente.Contato = cliente.Contato;
                clienteExistente.Telefone = cliente.Telefone;
                clienteExistente.Celular = cliente.Celular;
                clienteExistente.Email = cliente.Email;
                clienteExistente.Senha = cliente.Senha;
                clienteExistente.Fornecedor = cliente.Fornecedor;
                clienteExistente.PessoaFisica = cliente.PessoaFisica;
                clienteExistente.Cep = cliente.Cep;
                clienteExistente.Rua = cliente.Rua;
                clienteExistente.NumeroRua = cliente.NumeroRua;
                clienteExistente.Complemento = cliente.Complemento;
                clienteExistente.Bairro = cliente.Bairro;
                clienteExistente.Cidade = cliente.Cidade;
                clienteExistente.Estado = cliente.Estado;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<ClienteModel> Excluir(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }

            return cliente;
        }
    }
}