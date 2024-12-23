using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class ItensVendaModelConfig : IEntityTypeConfiguration<ItensVendaModel>
    {
        public void Configure(EntityTypeBuilder<ItensVendaModel> builder)
        {
            // Relacionamento Itens -> Venda
            builder.HasOne(o => o.Venda)
                   .WithMany(c => c.Itens)
                   .HasForeignKey(o => o.VendaId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Vendas -> Usuário
            builder.HasOne(o => o.Produto)
                   .WithMany(u => u.Itens)
                   .HasForeignKey(o => o.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Preco)
                .HasPrecision(15, 2);
        }
    }
}
