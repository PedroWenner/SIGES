using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SIGES.Models
{
    public class ClienteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O documento é obrigatório.")]
        [MaxLength(20)]
        public string? Documento { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(255)]
        public string Nome { get; set; }

        [MaxLength(20)]
        public string? Sexo { get; set; }

        [Required]
        [Range(0, 1)]
        public int PessoaFisica { get; set; } = 1;

        [MaxLength(45)]
        public string? Contato { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(20)]
        public string? Celular { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MaxLength(255)]
        public string Senha { get; set; }

        [Required]
        [Range(0, 1)]
        public int Fornecedor { get; set; } = 0;

        [MaxLength(20)]
        public string? Cep { get; set; }

        [MaxLength(70)]
        public string? Rua { get; set; }

        [MaxLength(15)]
        public string? NumeroRua { get; set; }

        [MaxLength(45)]
        public string? Complemento { get; set; }

        [MaxLength(45)]
        public string? Bairro { get; set; }

        [MaxLength(45)]
        public string? Cidade { get; set; }

        [MaxLength(20)]
        public string? Estado { get; set; }

        public int UsuarioCadastro { get; set; } = 1;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [ValidateNever]
        public ICollection<PedidoModel> Pedidos { get; set; }

    }
}
