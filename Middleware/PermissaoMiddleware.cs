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
                var master = context.User.Claims.FirstOrDefault(c => c.Type == "Master")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    var permissoes = await permissaoRepositorio.ObterPermissoesUsuario(userId);
                    context.Items["Permissoes"] = permissoes;
                    context.Items["Master"] = master;
                }
            }

            await _next(context);
        }
    }
}
