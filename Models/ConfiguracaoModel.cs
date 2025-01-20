using Microsoft.EntityFrameworkCore;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    [Index(nameof(Config), IsUnique = true)]
    public class ConfiguracaoModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "A configuração é obrigatória.")]
        public string Config { get; set; }

        public string? Valor { get; set; }
    }
}