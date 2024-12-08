using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;

namespace SIGES.Models
{
    public class UsuarioModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }

        public string? RgNumero { get; set; }

        [Required(ErrorMessage = "O cpf é obrigatório.")]
        public string Cpf {  get; set; }

        public string? Telefone { get; set; }

        [StringLength(80)]
        public string? Email { get; set; }

        [IgnoreLog]
        [SenhaObrigatoriaNoCadastro]
        [MaxLength(255)]
        public string? Senha { get; set; }

        [MaxLength(20)]
        public string? Cep { get; set; }

        [MaxLength(70)]
        public string? Rua { get; set; }

        [MaxLength(15)]
        public string? NumeroRua { get; set; }

        [MaxLength(45)]
        public string? Bairro { get; set; }

        [MaxLength(45)]
        public string? Cidade { get; set; }

        [MaxLength(20)]
        public string? Estado { get; set; }

        public DateTime? DataExpiracao { get; set; }

        public int? Situacao { get; set; }

        [IgnoreLog]
        public byte[]? ImagemBlob { get; set; }

        [Required(ErrorMessage = "A permissão é obrigatória.")]
        public int PermissaoId { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public PermissaoModel Permissao { get; set; }
    }
}
