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
            builder.HasOne(s => s.Clientes)
                .WithMany(u => u.Ordens)
                .HasForeignKey(s => s.ClientesId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento OS -> Usuário
            builder.HasOne(o => o.Usuarios)
                .WithMany(u => u.Ordens)
                .HasForeignKey(o => o.UsuariosId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento OS -> Garantia (Opcional)
            builder.HasOne(o => o.Garantias)
                .WithMany(g => g.Ordens) // Garantia pode estar em várias OS, mas sem navegação reversa explícita
                .HasForeignKey(o => o.GarantiasId)
                .OnDelete(DeleteBehavior.SetNull); // Define como NULL se a garantia for excluída

            // Relacionamento OS -> ServicosOs ()
            builder.HasMany(o => o.Servicos)
                .WithOne(s => s.Os)
                .HasForeignKey(s => s.OsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}