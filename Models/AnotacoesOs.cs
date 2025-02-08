using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    public class AnotacoesOs
    {
        [Key]
        [Column("id_anotacoes")]
        public int Id { get; set; }

        [Column("anotacao")]
        public string Anotacao { get; set; }

        [Column("data_hora")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Column("os_id")]
        public int OsId { get; set; }

        public virtual OsModel? Os { get; set; }
    }
}
