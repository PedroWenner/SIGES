using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class ArquivoModel
    {
        [Key]
        [Column("idDocumentos")]
        public int Id { get; set; }

        [Column("documento")]
        [MaxLength(70)]
        public string? Documento { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("file")]
        [MaxLength(100)]
        public string? File { get; set; }

        [Column("path")]
        [MaxLength(300)]
        public string? Path { get; set; }

        [Column("url")]
        [MaxLength(300)]
        public string? Url { get; set; }

        [Column("cadastro")]
        public DateTime? Cadastro { get; set; } = DateTime.Now;

        [Column("tipo")]
        [MaxLength(15)]
        public string? Tipo { get; set; }

        [Column("tamanho")]
        [MaxLength(45)]
        public string? Tamanho { get; set; }
    }
}
