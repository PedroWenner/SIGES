using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class ClienteModelConfig : IEntityTypeConfiguration<ClienteModel>
    {
        public void Configure(EntityTypeBuilder<ClienteModel> builder)
        {
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.Documento)
                .IsUnique();
        }
    }
}