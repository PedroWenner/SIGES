using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class ProdutosOs
    {
        [Key]
        [Column("idProdutos_os")]
        public int Id { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Column("descricao")]
        [MaxLength(80)]
        public string? Descricao { get; set; }

        [Column("preco", TypeName = "decimal(15,2)")]
        public decimal? Preco { get; set; } = 0;

        [Column("os_id")]
        [Required]
        public int OsId { get; set; }

        [Column("produtos_id")]
        [Required]
        public int ProdutosId { get; set; }

        [Column("sub_total", TypeName = "decimal(15,2)")]
        public decimal SubTotal { get; set; } = 0;

        public virtual OsModel? Os { get; set; }
        public virtual ProdutoModel? Produtos { get; set; }
    }
}
