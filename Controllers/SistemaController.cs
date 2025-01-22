using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
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

        public SistemaController(ILogService logServiceRepositorio, IConfiguracao configuracaoRepositorio, AppSettingsService appSettingsService)
        {
            _logServiceRepositorio = logServiceRepositorio;
            _configuracaoRepositorio = configuracaoRepositorio;
            _appSettingsService = appSettingsService;
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
    }
}