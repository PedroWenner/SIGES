using Microsoft.Extensions.Configuration;

namespace SIGES.Services
{
    public class AppSettingsService
    {
        private readonly IConfiguration _configuration;

        public AppSettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObterConfiguracao(string chave)
        {
            return _configuration[$"AppSettings:{chave}"] ?? string.Empty;
        }

        public bool ObterConfiguracaoBoolean(string chave)
        {
            return bool.TryParse(_configuration[$"AppSettings:{chave}"], out bool result) && result;
        }

        public List<string> ObterConfiguracaoLista(string chave)
        {
            var valor = _configuration.GetSection($"AppSettings:{chave}").Get<List<string>>();
            return valor ?? new List<string>();
        }
    }
}