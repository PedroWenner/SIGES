using SIGES.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SIGES.Interfaces;
using SIGES.Seeders;

namespace SIGES.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas as configurações de entidades definidas no assembly atual
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Aplica os seeders
            AplicarSeeders(modelBuilder);
        }

        private void AplicarSeeders(ModelBuilder modelBuilder)
        {
            var seeders = new ISeeder[]
            {
                new UsuarioSeeder()
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(modelBuilder);
            }
        }

        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PermissaoModel> Permissoes { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<LogAlteracaoModel> LogsOperacoes { get; set; }
        public DbSet<ServicoModel> Servicos { get; set; }
        public DbSet<OsModel> Ordens { get; set; }
        public DbSet<VendaModel> Vendas { get; set; }
        public DbSet<LancamentoModel> Lancamentos { get; set; }
        public DbSet<ItensVendaModel> ItensVenda { get; set; }
        public DbSet<EmitenteModel> Emitente { get; set; }
        public DbSet<GarantiaModel> Garantias { get; set; }
    }
}
