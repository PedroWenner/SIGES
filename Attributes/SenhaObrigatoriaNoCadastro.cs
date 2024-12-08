using SIGES.Models;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Attributes
{
    public class SenhaObrigatoriaNoCadastro : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var usuario = (UsuarioModel)validationContext.ObjectInstance;

            // Verifica se a senha é obrigatória somente para novos usuários
            if (usuario.Id == 0 && string.IsNullOrEmpty(usuario.Senha))
            {
                return new ValidationResult("A senha é obrigatória para novos usuários.");
            }

            return ValidationResult.Success;
        }
    }
}
