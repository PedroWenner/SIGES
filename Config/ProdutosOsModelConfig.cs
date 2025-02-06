using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class ProdutosOsModelConfig : IEntityTypeConfiguration<ProdutosOs>
    {
        public void Configure(EntityTypeBuilder<ProdutosOs> builder)
        {
            // Relacionamento ProdutosOs -> Produtos
            builder.HasOne(s => s.Produtos)
                .WithMany(o => o.ProdutosOs)
                .HasForeignKey(s => s.ProdutosId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento ProdutosOs -> OS (Ordem de Serviço)
            builder.HasOne(s => s.Os)
                .WithMany(o => o.Produtos)
                .HasForeignKey(s => s.OsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
