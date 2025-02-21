using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGES.Models;

namespace SIGES.Config
{
    public class VendaModelConfig : IEntityTypeConfiguration<VendaModel>
    {
        public void Configure(EntityTypeBuilder<VendaModel> builder)
        {
            // Relacionamento Venda -> Cliente
            builder.HasOne(o => o.Cliente)
                   .WithMany(c => c.Vendas)
                   .HasForeignKey(o => o.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Vendas -> Usuário
            builder.HasOne(o => o.Usuario)
                   .WithMany(u => u.Vendas)
                   .HasForeignKey(o => o.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Vendas -> Lançamentos
            builder.HasOne(o => o.Lancamento)
                   .WithMany(l => l.Vendas)
                   .HasForeignKey(o => o.LancamentoId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
