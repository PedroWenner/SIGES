using SIGES.Interfaces;

namespace SIGES.Middleware
{
    public class PermissaoMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissaoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPermissaoRepositorio permissaoRepositorio)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    // Obter permissões do usuário
                    var permissoes = await permissaoRepositorio.ObterPermissoesUsuario(userId);

                    // Armazenar as permissões como uma lista no contexto
                    context.Items["Permissoes"] = permissoes;
                }
            }

            await _next(context);
        }
    }
}
