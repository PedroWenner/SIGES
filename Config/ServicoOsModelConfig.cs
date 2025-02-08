using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class ServicoOsModelConfig : IEntityTypeConfiguration<ServicosOs>
    {
        public void Configure(EntityTypeBuilder<ServicosOs> builder)
        {
            // Relacionamento ServiçoOs -> Servico
            builder.HasOne(s => s.Servicos)
                .WithMany(o => o.ServicosOs)
                .HasForeignKey(s => s.ServicosId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento ServiçoOs -> OS (Ordem de Serviço)
            builder.HasOne(s => s.Os)
                .WithMany(o => o.Servicos)
                .HasForeignKey(s => s.OsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
