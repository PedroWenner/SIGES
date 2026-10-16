using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SIGES.Data;
using SIGES.Services;
using System.Security.Claims;

namespace SIGES.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;

        public LoginController(AuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login/Autenticar")]
        public async Task<JsonResult> Autenticar(string login, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(/*u => u.Email == login*/);
            //if (usuario == null || !_authService.VerificarSenha(usuario.Senha, senha))
            //{
            //    return Json(new { sucesso = false, mensagem = "Os dados de acesso estão incorretos. Por favor, tente novamente." });
            //}

            //if (usuario.DataExpiracao < DateTime.Now)
            //{
            //    return Json(new { sucesso = false, mensagem = "O usuário que você está utilizando para acesso expirou. Contate seu superior." });
            //}

            // Criação do cookie de autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("UserId", usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.PermissaoId.ToString()),
                new Claim("Master", usuario.Master)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Armazenar dados adicionais na sessão
            HttpContext.Session.SetString("UserId", usuario.Id.ToString());
            HttpContext.Session.SetString("UserName", usuario.Nome);
            HttpContext.Session.SetString("UserCpf", usuario.Cpf);
            // Armazenar a imagem do usuário na sessão (caso exista)
            if (usuario.ImagemBlob != null)
            {
                var base64Image = Convert.ToBase64String(usuario.ImagemBlob);
                HttpContext.Session.SetString("UserImage", base64Image);
            }
            else
            {
                HttpContext.Session.Remove("UserImage");
            }

            return Json(new { sucesso = true });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
