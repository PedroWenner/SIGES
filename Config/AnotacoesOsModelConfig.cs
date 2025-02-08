using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class AnotacoesOsModelConfig : IEntityTypeConfiguration<AnotacoesOs>
    {
        public void Configure(EntityTypeBuilder<AnotacoesOs> builder)
        {
            // Relacionamento AnotacoesOs -> OS
            builder.HasOne(s => s.Os)
                .WithMany(o => o.Anotacoes)
                .HasForeignKey(s => s.OsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
