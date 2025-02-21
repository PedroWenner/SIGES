using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class LancamentoModelConfig : IEntityTypeConfiguration<LancamentoModel>
    {
        public void Configure(EntityTypeBuilder<LancamentoModel> builder)
        {
            // Relacionamento Lançamento -> Cliente
            builder.HasOne(o => o.Clientes)
                   .WithMany(c => c.Lancamentos)
                   .HasForeignKey(o => o.ClientesId);

            // Relacionamento Lançamento -> Categorias
            builder.HasOne(o => o.Categorias)
                   .WithMany(u => u.Lancamentos)
                   .HasForeignKey(o => o.CategoriasId);

            // Relacionamento Lançamento -> Vendas
            builder.HasOne(o => o.Venda)
                   .WithMany(l => l.Lancamentos)
                   .HasForeignKey(o => o.VendasId);

            // Relacionamento Lançamento -> Vendas
            builder.HasOne(o => o.Contas)
                   .WithMany(l => l.Lancamentos)
                   .HasForeignKey(o => o.ContasId);

            // Relacionamento Lançamento -> Vendas
            builder.HasOne(o => o.Usuarios)
                   .WithMany(l => l.Lancamentos)
                   .HasForeignKey(o => o.UsuariosId);

            // Relacionamento Lançamento -> Os
            builder.HasOne(o => o.Os)
                   .WithMany(l => l.Lancamentos)
                   .HasForeignKey(o => o.OsId);
        }
    }
}
