using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Repositorios;
using SIGES.Services;
using System.Text.Json;

namespace SIGES.Helpers
{
    public static class ConfiguracaoHelper
    {
        private static Dictionary<string, string> _configuracoes = new Dictionary<string, string>();

        public static void CarregarConfiguracoes(IConfiguracao configuracaoRepositorio, AppSettingsService appSettingsService)
        {
            var configuracoes = configuracaoRepositorio.ObterTodasConfiguracoesAsync().Result;

            foreach (var config in configuracoes)
            {
                _configuracoes[config.Config] = config.Valor ?? string.Empty;
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
                _configuracoes[chave] = appSettingsService.ObterConfiguracao(chave);
            }

            var osStatusList = appSettingsService.ObterConfiguracaoLista("os_status_list");
            if (!_configuracoes.ContainsKey("os_status_list"))
            {
                _configuracoes["os_status_list"] = JsonSerializer.Serialize(osStatusList);
            }
        }

        public static string ObterValor(string chave)
        {
            return _configuracoes.ContainsKey(chave) ? _configuracoes[chave] : string.Empty;
        }
    }
}
