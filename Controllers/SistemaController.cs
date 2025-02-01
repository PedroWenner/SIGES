using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models.Requests;
using SIGES.Repositorios;
using SIGES.Services;
using System.Text.Json;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("configurar")]
    public class SistemaController : Controller
    {
        private readonly ILogService _logServiceRepositorio;
        private readonly IConfiguracao _configuracaoRepositorio;
        private readonly AppSettingsService _appSettingsService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public SistemaController(ILogService logServiceRepositorio, IConfiguracao configuracaoRepositorio, AppSettingsService appSettingsService, IUsuarioRepositorio usuarioRepositorio)
        {
            _logServiceRepositorio = logServiceRepositorio;
            _configuracaoRepositorio = configuracaoRepositorio;
            _appSettingsService = appSettingsService;
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var configuracoes = await _configuracaoRepositorio.ObterTodasConfiguracoesAsync();
            var configuracoesViewModel = new Dictionary<string, string>();
            foreach (var config in configuracoes)
            {
                configuracoesViewModel[config.Config] = config.Valor ?? string.Empty;
            }

            // Lista de configurações que devem ser carregadas do appsettings.json
            var configuracoesAppSettings = new List<string>
            {
                "IMPRIMIR_ANEXOS",
                "PAYMENT_GATEWAYS_EFI_PRODUCTION",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_ID",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_EFI_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_PUBLIC_KEY",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_ACCESS_TOKEN",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_ID",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_ASAAS_PRODUCTION",
                "PAYMENT_GATEWAYS_ASAAS_NOTIFY",
                "PAYMENT_GATEWAYS_ASAAS_CREDENTIAIS_API_KEY",
                "PAYMENT_GATEWAYS_ASAAS_BOLETO_EXPIRATION",
                "API_ENABLED",
                "API_TOKEN_EXPIRE_TIME",
                "API_JWT_KEY",
                "EMAIL_PROTOCOL",
                "EMAIL_SMTP_HOST",
                "EMAIL_SMTP_CRYPTO",
                "EMAIL_SMTP_PORT",
                "EMAIL_SMTP_USER",
                "EMAIL_SMTP_PASS"
            };

            // Carregar as configurações do appsettings.json
            foreach (var chave in configuracoesAppSettings)
            {
                configuracoesViewModel[chave] = _appSettingsService.ObterConfiguracao(chave);
            }

            // Carregar valores do appsettings.json
            var osStatusList = _appSettingsService.ObterConfiguracaoLista("os_status_list");
            // Se a configuração não estiver no banco, pegamos do appsettings.json
            if (!configuracoesViewModel.ContainsKey("os_status_list"))
            {
                configuracoesViewModel["os_status_list"] = JsonSerializer.Serialize(osStatusList);
            }

            return View("~/Views/Alca/Configurar.cshtml", configuracoesViewModel);
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> SalvarConfiguracoes([FromForm] Dictionary<string, string> configuracoes, [FromForm] List<string> os_status_list)
        {
            var configuracoesAppSettings = new HashSet<string>
            {
                "IMPRIMIR_ANEXOS",
                "PAYMENT_GATEWAYS_EFI_PRODUCTION",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_ID",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_EFI_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_PUBLIC_KEY",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_ACCESS_TOKEN",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_ID",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_ASAAS_PRODUCTION",
                "PAYMENT_GATEWAYS_ASAAS_NOTIFY",
                "PAYMENT_GATEWAYS_ASAAS_CREDENTIAIS_API_KEY",
                "PAYMENT_GATEWAYS_ASAAS_BOLETO_EXPIRATION",
                "API_ENABLED",
                "API_TOKEN_EXPIRE_TIME",
                "API_JWT_KEY",
                "EMAIL_PROTOCOL",
                "EMAIL_SMTP_HOST",
                "EMAIL_SMTP_CRYPTO",
                "EMAIL_SMTP_PORT",
                "EMAIL_SMTP_USER",
                "EMAIL_SMTP_PASS"
            };

            foreach (var item in configuracoes)
            {
                if (configuracoesAppSettings.Contains(item.Key))
                {
                    AtualizarAppSettings(item.Key, item.Value);
                }
                else
                {
                    await _configuracaoRepositorio.AtualizarConfiguracaoAsync(item.Key, item.Value);
                }
            }

            // Salvar os checkboxes como JSON
            var osStatusJson = System.Text.Json.JsonSerializer.Serialize(os_status_list);
            await _configuracaoRepositorio.AtualizarConfiguracaoAsync("os_status_list", osStatusJson);

            TempData["Success"] = "Configurações atualizadas com sucesso!";
            return RedirectToAction("Index");
        }

        private void AtualizarAppSettings(string chave, string valor)
        {
            var appSettingsFile = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            if (!System.IO.File.Exists(appSettingsFile))
            {
                throw new FileNotFoundException("O arquivo appsettings.json não foi encontrado.");
            }

            // Ler o conteúdo atual do appsettings.json
            var json = System.IO.File.ReadAllText(appSettingsFile);
            var jsonObj = Newtonsoft.Json.Linq.JObject.Parse(json);

            // Verificar se a seção "AppSettings" existe
            var section = jsonObj["AppSettings"] ?? new Newtonsoft.Json.Linq.JObject();

            // Atualizar ou adicionar a chave
            section[chave] = valor;

            // Aplicar a alteração
            jsonObj["AppSettings"] = section;

            // Escrever de volta no arquivo
            System.IO.File.WriteAllText(appSettingsFile, jsonObj.ToString(Newtonsoft.Json.Formatting.Indented));
        }

        [HttpGet("obter-configuracoes")]
        public async Task<IActionResult> ObterConfiguracoes()
        {
            var configuracoes = await _configuracaoRepositorio.ObterTodasConfiguracoesAsync();
            var configuracoesViewModel = new Dictionary<string, string>();

            foreach (var config in configuracoes)
            {
                configuracoesViewModel[config.Config] = config.Valor ?? string.Empty;
            }

            var configuracoesAppSettings = new List<string>
            {
                "IMPRIMIR_ANEXOS",
                "PAYMENT_GATEWAYS_EFI_PRODUCTION",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_ID",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_EFI_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_PUBLIC_KEY",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_ACCESS_TOKEN",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_ID",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_ASAAS_PRODUCTION",
                "PAYMENT_GATEWAYS_ASAAS_NOTIFY",
                "PAYMENT_GATEWAYS_ASAAS_CREDENTIAIS_API_KEY",
                "PAYMENT_GATEWAYS_ASAAS_BOLETO_EXPIRATION",
                "API_ENABLED",
                "API_TOKEN_EXPIRE_TIME",
                "API_JWT_KEY",
                "EMAIL_PROTOCOL",
                "EMAIL_SMTP_HOST",
                "EMAIL_SMTP_CRYPTO",
                "EMAIL_SMTP_PORT",
                "EMAIL_SMTP_USER",
                "EMAIL_SMTP_PASS"
            };

            foreach (var chave in configuracoesAppSettings)
            {
                if (!configuracoesViewModel.ContainsKey(chave))
                {
                    configuracoesViewModel[chave] = _appSettingsService.ObterConfiguracao(chave);
                }
            }

            return Json(configuracoesViewModel);
        }

        [HttpGet("minha-conta")]
        public async Task<IActionResult> MinhaConta()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                TempData["Error"] = "Usuário não identificado!";
                return RedirectToAction("Index", "Home");
            }

            var usuario = await _usuarioRepositorio.ObterPorId(id);
            if (usuario == null)
            {
                TempData["Error"] = "Usuário não encontrado!";
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Alca/MinhaConta.cshtml", usuario);
        }

        [HttpPost("atualizar-senha")]
        public async Task<IActionResult> AtualizarSenha([FromForm] string oldSenha, [FromForm] string novaSenha, [FromForm] string confirmarSenha)
        {
            try
            {
                var usuarioId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (usuarioId == null)
                {
                    return Json(new { status = "error", message = "Usuário não autenticado." });
                }

                var usuario = await _usuarioRepositorio.ObterPorId(int.Parse(usuarioId));
                if (usuario == null)
                {
                    return Json(new { status = "error", message = "Usuário não encontrado." });
                }

                if (!BCrypt.Net.BCrypt.Verify(oldSenha, usuario.Senha))
                {
                    return Json(new { status = "error", message = "Senha atual incorreta." });
                }

                if (novaSenha != confirmarSenha)
                {
                    return Json(new { status = "error", message = "As senhas não coincidem." });
                }

                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
                await _usuarioRepositorio.Atualizar(usuario);

                return Json(new { status = "success", message = "Senha atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("upload-imagem-usuario")]
        public async Task<IActionResult> UploadUserImage(IFormFile? userfile)
        {
            try
            {
                var usuarioId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (usuarioId == null)
                {
                    return Json(new { status = "error", message = "Usuário não autenticado." });
                }

                var usuario = await _usuarioRepositorio.ObterPorId(int.Parse(usuarioId));
                if (usuario == null)
                {
                    return Json(new { status = "error", message = "Usuário não encontrado." });
                }

                if (userfile != null && userfile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await userfile.CopyToAsync(memoryStream);
                        usuario.ImagemBlob = memoryStream.ToArray();
                    }

                    await _usuarioRepositorio.Atualizar(usuario);
                    return Json(new { status = "success", message = "Imagem atualizada com sucesso!" });
                }

                return Json(new { status = "error", message = "Nenhuma imagem selecionada!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = $"Erro ao processar a imagem: {ex.Message}" });
            }
        }
    }
}