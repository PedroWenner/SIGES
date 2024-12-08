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
            modelBuilder.Entity<LancamentoModel>()
                .Property(l => l.Observacao)
                .IsRequired(false);

            modelBuilder.Entity<LancamentoModel>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(l => l.CategoriaId);

            modelBuilder.Entity<ProdutoModel>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(l => l.CategoriaId);

            modelBuilder.Entity<UsuarioModel>()
                .HasOne(l => l.Permissao)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(l => l.PermissaoId);

            //// Seed para a tabela de Permissões
            //modelBuilder.Entity<PermissaoModel>().HasData(
            //    new PermissaoModel
            //    {
            //        Id = 1,
            //        Nome = "Admin"
            //    }
            //);

            //// Seed para a tabela de Usuários (com senha hasheada)
            //modelBuilder.Entity<UsuarioModel>().HasData(
            //    new UsuarioModel
            //    {
            //        Id = 1,
            //        Nome = "Administrador",
            //        Email = "teste@teste.com",
            //        Cpf = "000.000.000-00",
            //        Senha = BCrypt.Net.BCrypt.HashPassword("123"), // Substitua por uma senha segura
            //        PermissaoId = 1, // Vinculado à permissão "Admin"
            //        DataCadastro = DateTime.Now,
            //        UsuarioCadastro = 1,
            //        Situacao = 1 // Ativo
            //    }
            //);
        }

        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<LancamentoModel> Lancamentos { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PermissaoModel> Permissoes { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<LogAlteracaoModel> LogsOperacoes { get; set; }
    }
}
