using SIGES.Interfaces;

namespace SIGES.Services
{
    public class TemaService
    {
        private readonly IConfiguracao _configuracaoRepositorio;

        public TemaService(IConfiguracao configuracaoRepositorio)
        {
            _configuracaoRepositorio = configuracaoRepositorio;
        }

        public async Task<string> ObterTemaAsync()
        {
            var temaBanco = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("app_theme");
            return !string.IsNullOrEmpty(temaBanco) ? temaBanco : "default";
        }
    }
}
