using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class ServicoModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do serviço é obrigatório.")]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório.")]
        public decimal Valor { get; set; }
    }
}
