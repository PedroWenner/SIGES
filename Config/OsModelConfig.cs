using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class OsModelConfig : IEntityTypeConfiguration<OsModel>
    {
        public void Configure(EntityTypeBuilder<OsModel> builder)
        {
            // Relacionamento OS -> Cliente
            builder.HasOne(o => o.Cliente)
                   .WithMany(c => c.Ordens)
                   .HasForeignKey(o => o.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento OS -> Usuário
            builder.HasOne(o => o.Usuario)
                   .WithMany(u => u.Ordens)
                   .HasForeignKey(o => o.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
