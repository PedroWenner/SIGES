using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }
        public int Status { get; set; }

        [Required(ErrorMessage = "Selecione o produto.")]
        public int ProdutoId { get; set; }

        [ValidateNever]
        public ProdutoModel Produto { get; set; }

        public int Qtd { get; set; }
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Selecione o produto.")]
        public int ClienteId { get; set; }

        [ValidateNever]
        public ClienteModel Cliente { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;
    }
}