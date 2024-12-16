using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;
using System.Reflection.Emit;

namespace SIGES.Config
{
    public class ProdutoModelConfig : IEntityTypeConfiguration<ProdutoModel>
    {
        public void Configure(EntityTypeBuilder<ProdutoModel> builder)
        {
            // Relacionamento Produto -> Categoria
            builder.HasOne(o => o.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(o => o.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
