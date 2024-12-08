using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class CategoriaModel
    {
        [IgnoreLog]
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }

        [IgnoreLog]
        public byte[]? ImagemBlob { get; set; } // Campo para armazenar a imagem como blob

        [IgnoreLog]
        [ValidateNever]
        public ICollection<LancamentoModel> Lancamentos { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ICollection<ProdutoModel> Produtos { get; set; }

        [IgnoreLog]
        [NotMapped] // Campo não mapeado no banco
        public int ProximoCodigo { get; set; }
    }
}
