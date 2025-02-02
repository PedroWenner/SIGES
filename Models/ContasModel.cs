using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SIGES.Models
{
    public class ContasModel
    {
        [IgnoreLog]
        [Key]
        [Column("idContas")]
        public int Id { get; set; }

        [Column("conta")]
        public string? Conta { get; set; }

        [Column("banco")]
        public string? Banco { get; set; }

        [Column("numero")]
        public string? Numero { get; set; }

        [Column("saldo")]
        public string? Saldo { get; set; }

        [Column("cadastro")]
        public DateTime? Cadastro { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("tipo")]
        public string? Tipo { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ICollection<LancamentoModel> Lancamentos { get; set; } = new List<LancamentoModel>();
    }
}
