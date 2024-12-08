using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        
        public int Status { get; set; }

        public int? Codigo { get; set; }
        
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        
        public decimal? Peso { get; set; }
        
        public string? UnidadeMedida { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int CategoriaId { get; set; }

        [ValidateNever]
        public CategoriaModel Categoria { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }
        
        public string? Observacao { get; set; }
        
        // Para a imagem do produto
        public string? CaminhoImagem { get; set; }

        public int UsuarioCadastro { get; set; } = 1;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [ValidateNever]
        public ICollection<PedidoModel> Pedidos { get; set; }
    }
}