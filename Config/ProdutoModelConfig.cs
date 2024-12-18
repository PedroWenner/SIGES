using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class ProdutoModelConfig : IEntityTypeConfiguration<ProdutoModel>
    {
        public void Configure(EntityTypeBuilder<ProdutoModel> builder)
        {
            builder.Property(e => e.PrecoCompra)
                .HasPrecision(15, 2);

            builder.Property(e => e.MargemLucro)
                .HasPrecision(15, 2);

            builder.Property(e => e.PrecoVenda)
                .HasPrecision(15, 2);
        }
    }
}
