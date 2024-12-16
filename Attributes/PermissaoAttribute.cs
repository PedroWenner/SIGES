using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SIGES.Models;

namespace SIGES.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissaoAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissao;

        public PermissaoAttribute(string permissao)
        {
            _permissao = permissao;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                RedirecionarComMensagem(context, "Usuário não autenticado. Faça login novamente.");
                return;
            }

            // Obter permissões desserializadas do contexto
            var permissoes = context.HttpContext.Items["Permissoes"] as List<PermissaoDetalheModel>;

            // Verificar se o usuário possui a permissão necessária
            if (permissoes == null || !permissoes.Any(p => p.NomePermissao == _permissao))
            {
                if (!permissoes.Any(p => p.Ativo == 1))
                {
                    RedirecionarComMensagem(context, "Permissão utilizada não está mais ativa.");
                }

                RedirecionarComMensagem(context, "Você não tem permissão para acessar esta funcionalidade.");
            }
        }

        private void RedirecionarComMensagem(AuthorizationFilterContext context, string mensagem)
        {
            context.HttpContext.Session.SetString("PermissaoMensagem", mensagem);
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
