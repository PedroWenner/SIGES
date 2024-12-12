using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;

namespace SIGES.Models
{
    public class ClienteModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O documento é obrigatório.")]
        public string? Documento { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        public string? Sexo { get; set; }

        [Required]
        [Range(0, 1)]
        public int PessoaFisica { get; set; } = 1;

        public string? Contato { get; set; }

        public string? Telefone { get; set; }

        public string? Celular { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }

        [Required]
        [Range(0, 1)]
        public int Fornecedor { get; set; } = 0;

        public string? Cep { get; set; }

        public string? Rua { get; set; }

        public string? NumeroRua { get; set; }

        public string? Complemento { get; set; }

        public string? Bairro { get; set; }

        public string? Cidade { get; set; }

        public string? Estado { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ICollection<PedidoModel> Pedidos { get; set; }

    }
}
