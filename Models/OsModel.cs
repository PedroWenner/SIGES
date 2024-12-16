using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SIGES.Models
{
    public class OsModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informar o cliente é obrigatório.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Informar o técnico responsável pela OS.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Informar o status da OS.")]
        public int Status {  get; set; }

        [Required(ErrorMessage = "Informar a data inicial da OS.")]
        public DateTime DataInicial { get; set; } = DateTime.Now;

        public DateTime? DataFinal { get; set; }

        public string? Observacao { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ClienteModel Cliente { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public UsuarioModel Usuario { get; set; }

    }
}
