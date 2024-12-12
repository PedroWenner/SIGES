using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class PedidoModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Status { get; set; }

        [Required(ErrorMessage = "Selecione o produto.")]
        public int ProdutoId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ProdutoModel Produto { get; set; }

        public int Qtd { get; set; }

        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Selecione o produto.")]
        public int ClienteId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ClienteModel Cliente { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;
    }
}