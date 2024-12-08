using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class PermissaoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80)]
        public string Nome { get; set; }

        public string? Permissoes { get; set; }

        public int? Situacao { get; set; }
        
        public DateTime? Data { get; set; }

        public int UsuarioCadastro { get; set; } = 1;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [ValidateNever]
        public ICollection<UsuarioModel> Usuarios { get; set; }

    }

    public class PermissaoDetalheModel
    {
        public string NomePermissao { get; set; }
        public int Ativo { get; set; }  // 1 para ativo, 0 para inativo
    }
}
