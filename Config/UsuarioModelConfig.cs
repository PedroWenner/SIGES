using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class UsuarioModelConfig : IEntityTypeConfiguration<UsuarioModel>
    {
        public void Configure(EntityTypeBuilder<UsuarioModel> builder)
        {
            // Relacionamento Usuario -> Permissao
            builder.HasOne(u => u.Permissao)
                   .WithMany(p => p.Usuarios)
                   .HasForeignKey(u => u.PermissaoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.Cpf)
                .IsUnique();
        }
    }
}