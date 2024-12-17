using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(0, 1)]
        public int TipoMovimento { get; set; }

        public decimal PrecoCompra { get; set; }

        public decimal? MargemLucro { get; set; }

        public decimal PrecoVenda { get; set; }

        public string? UnidadeMedida { get; set; }

        public int Estoque {  get; set; }

        public int? EstoqueMin {  get; set; }

        [IgnoreLog]
        public byte[]? ImagemBlob { get; set; }
    }
}