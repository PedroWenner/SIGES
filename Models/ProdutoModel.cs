using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class ProdutoModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int Status { get; set; }

        public int? Codigo { get; set; }
        
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        
        public decimal? Peso { get; set; }
        
        public string? UnidadeMedida { get; set; }

        [IgnoreLog]
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int CategoriaId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public CategoriaModel Categoria { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }
        
        public string? Observacao { get; set; }

        [IgnoreLog]
        public byte[]? ImagemBlob { get; set; }
    }
}