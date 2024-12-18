using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class LancamentoModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Descricao { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal Valor { get; set; }

        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; }

        public int? Baixado { get; set; } = 0;

        public string? ClienteFornecedor { get; set; }

        public string? FormaPgto { get; set; }

        public string? Tipo { get; set; }

        public string? AnexoBlob { get; set; }

        [IgnoreLog]
        public int? ClienteId { get; set; }
        
        [IgnoreLog]
        [ValidateNever]
        public ClienteModel? Cliente { get; set; }

        [IgnoreLog]
        public int? CategoriaId { get; set; }
        
        [IgnoreLog]
        [ValidateNever]
        public CategoriaModel? Categoria { get; set; }

        [IgnoreLog]
        public int? VendaId { get; set; }
        
        [IgnoreLog]
        [ValidateNever]
        public VendaModel? Venda { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ICollection<VendaModel> Vendas { get; set; } = new List<VendaModel>();

    }
}
