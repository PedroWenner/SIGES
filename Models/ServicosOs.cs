using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class ServicosOs
    {
        [Key]
        [Column("idServicos")]
        public int Id { get; set; }

        [Column("servico")]
        [MaxLength(80)]
        public string? Servico { get; set; }

        [Column("quantidade")]
        public double? Quantidade { get; set; }

        [Column("preco", TypeName = "decimal(15,2)")]
        public decimal? Preco { get; set; } = 0;

        [Column("os_id")]
        [Required]
        public int OsId { get; set; }

        [Column("servicos_id")]
        [Required]
        public int ServicosId { get; set; }

        [Column("subTotal", TypeName = "decimal(15,2)")]
        public decimal? SubTotal { get; set; } = 0;

        public virtual OsModel? Os { get; set; }
        public virtual ServicoModel? Servicos { get; set; }
    }
}
