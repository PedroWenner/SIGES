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
            builder.HasOne(o => o.Cliente)
                   .WithMany(c => c.Lancamentos)
                   .HasForeignKey(o => o.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Lançamento -> Categorias
            builder.HasOne(o => o.Categoria)
                   .WithMany(u => u.Lancamentos)
                   .HasForeignKey(o => o.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Lançamento -> Vendas
            builder.HasOne(o => o.Venda)
                   .WithMany(l => l.Lancamentos)
                   .HasForeignKey(o => o.VendaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Valor)
                .HasPrecision(15, 2);
        }
    }
}
