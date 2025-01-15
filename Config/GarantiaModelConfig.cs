using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class GarantiaModelConfig : IEntityTypeConfiguration<GarantiaModel>
    {
        public void Configure(EntityTypeBuilder<GarantiaModel> builder)
        {
            // Relacionamento Garantia -> Usuario
            builder.HasOne(o => o.Usuario)
                   .WithMany(c => c.Garantias)
                   .HasForeignKey(o => o.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
