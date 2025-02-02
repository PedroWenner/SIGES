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
        public string Nome { get; set; }

        public string? RgNumero { get; set; }

        [Required(ErrorMessage = "O cpf é obrigatório.")]
        public string Cpf {  get; set; }

        public string? Telefone { get; set; }

        public string? Email { get; set; }

        [IgnoreLog]
        [SenhaObrigatoriaNoCadastro]
        public string? Senha { get; set; }

        public string? Cep { get; set; }

        public string? Rua { get; set; }

        public string? NumeroRua { get; set; }

        public string? Bairro { get; set; }

        public string? Cidade { get; set; }

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

        [IgnoreLog]
        [ValidateNever]
        public string Master { get; set; } = "N";

        [IgnoreLog]
        [ValidateNever]
        public ICollection<OsModel> Ordens { get; set; } = new List<OsModel>();

        [IgnoreLog]
        [ValidateNever]
        public ICollection<VendaModel> Vendas { get; set; } = new List<VendaModel>();

        [IgnoreLog]
        [ValidateNever]
        public ICollection<GarantiaModel> Garantias { get; set; } = new List<GarantiaModel>();

        [IgnoreLog]
        [ValidateNever]
        public ICollection<LancamentoModel> Lancamentos { get; set; } = new List<LancamentoModel>();
    }
}
