using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdutoModel>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(l => l.CategoriaId);

            modelBuilder.Entity<UsuarioModel>()
                .HasOne(l => l.Permissao)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(l => l.PermissaoId);

            // Seed para a tabela de Permissões
            modelBuilder.Entity<PermissaoModel>().HasData(
                new PermissaoModel
                {
                    Id = 1,
                    Nome = "Admin",
                    Situacao = 1
                }
            );

            // Seed para a tabela de Usuários (com senha hasheada)
            modelBuilder.Entity<UsuarioModel>().HasData(
                new UsuarioModel
                {
                    Id = 1,
                    Nome = "Admin",
                    Cpf = "000.000.000-00",
                    Email = "teste@teste.com",
                    Senha = BCrypt.Net.BCrypt.HashPassword("123"),
                    Situacao = 1,
                    PermissaoId = 1
                }
            );
        }

        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PermissaoModel> Permissoes { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<LogAlteracaoModel> LogsOperacoes { get; set; }
    }
}
