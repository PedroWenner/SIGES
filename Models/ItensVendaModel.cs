using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SIGES.Models
{
    public class ItensVendaModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column(TypeName = "decimal(15,2)")]
        public decimal? SubTotal { get; set; } = 0;

        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal Preco {  get; set; }

        [IgnoreLog]
        public int VendaId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public VendaModel Venda { get; set; }

        public int ProdutoId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ProdutoModel Produto { get; set; }
    }
}
