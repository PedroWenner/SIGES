using SIGES.Models;

namespace SIGES.Helpers
{
    public class PermissaoHelper
    {
        public static bool TemPermissao(HttpContext context, string nomePermissao)
        {
            var permissoes = context.Items["Permissoes"] as List<PermissaoDetalheModel>;
            return permissoes != null && permissoes.Any(p => p.NomePermissao == nomePermissao && p.Ativo == 1);
        }
    }
}
