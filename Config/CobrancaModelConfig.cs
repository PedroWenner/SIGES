using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class CobrancaModelConfig : IEntityTypeConfiguration<Cobranca>
    {
        public void Configure(EntityTypeBuilder<Cobranca> builder)
        {
            // Relacionamento Venda -> Cliente
            builder.HasOne(c => c.Venda)
                .WithMany()
                .HasForeignKey(c => c.VendasId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Vendas -> Usuário
            builder.HasOne(c => c.Os)
                .WithMany()
                .HasForeignKey(c => c.OsId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Vendas -> Lançamentos
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClientesId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
