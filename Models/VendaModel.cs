using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class VendaModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.Now;

        public string Status { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal? ValorTotal { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal? ValorDesconto { get; set; }

        public int? Faturado { get; set; }

        [IgnoreLog]
        public int ClienteId { get; set; }
        
        [IgnoreLog]
        [ValidateNever]
        public ClienteModel Cliente { get; set; }
        
        [IgnoreLog]
        public int UsuarioId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public UsuarioModel Usuario { get; set; }

        [IgnoreLog]
        public int? LancamentoId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public LancamentoModel Lancamento { get; set; }

        [IgnoreLog]
        public string? Observacao { get; set; }

        [IgnoreLog]
        public string? ObservacaoCliente { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ICollection<LancamentoModel> Lancamentos { get; set; } = new List<LancamentoModel>();

    }
}
