using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class ServicoModelConfig : IEntityTypeConfiguration<ServicoModel>
    {
        public void Configure(EntityTypeBuilder<ServicoModel> builder)
        {
            builder.Property(e => e.Valor)
                .HasPrecision(15, 2);
        }
    }
}
