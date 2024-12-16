using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        public async Task<IActionResult> Login(string login, string senha)
        {
            // Busca o usuário pelo login (email ou nome)
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == login || u.Nome == login);
            if (usuario == null || !_authService.VerificarSenha(usuario.Senha, senha))
            {
                ViewBag.Error = "Os dados de acesso estão incorretos. Por favor, tente novamente.";
                return View(usuario);
            }

            if (usuario.DataExpiracao < DateTime.Now)
            {
                ViewBag.Error = "O usuário que você está utilizando para acesso expirou. Contate seu superior.";
                return View(usuario);
            }

            // Criação do cookie de autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("UserId", usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.PermissaoId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Lembra o login
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Armazenar dados adicionais na sessão
            HttpContext.Session.SetString("UserName", usuario.Nome);
            HttpContext.Session.SetString("UserCpf", usuario.Cpf);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
