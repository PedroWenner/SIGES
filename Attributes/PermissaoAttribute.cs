using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SIGES.Models;

namespace SIGES.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissaoAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissao;
        private readonly string _mensagem = string.Empty;

        public PermissaoAttribute(string permissao, string mensagem = "")
        {
            _permissao = permissao;
            _mensagem = mensagem;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var master = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Master")?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                RedirecionarComMensagem(context, "Usuário não autenticado. Faça login novamente.");
                return;
            }

            // Obter permissões desserializadas do contexto
            var permissoes = context.HttpContext.Items["Permissoes"] as List<PermissaoDetalheModel>;

            // Verificar se o usuário possui a permissão necessária
            if (permissoes == null || (!permissoes.Any(p => p.NomePermissao == _permissao) || master == "N"))
            {
                if (!permissoes.Any(p => p.Ativo == 1))
                {
                    RedirecionarComMensagem(context, "Permissão utilizada não está mais ativa.");
                }

                string mensagem = _mensagem;
                if (string.IsNullOrEmpty(mensagem))
                    mensagem = "Você não tem permissão para acessar esta funcionalidade!";

                RedirecionarComMensagem(context, mensagem);
            }
        }

        private void RedirecionarComMensagem(AuthorizationFilterContext context, string mensagem)
        {
            context.HttpContext.Session.SetString("PermissaoMensagem", mensagem);
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
