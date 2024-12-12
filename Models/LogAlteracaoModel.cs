using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class LogAlteracaoModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Usuario { get; set; }

        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;

        public string Entidade { get; set; }

        public int EntidadeId { get; set; }

        public string Alteracoes { get; set; }

        public string TipoOperacao { get; set; }
    }
}
