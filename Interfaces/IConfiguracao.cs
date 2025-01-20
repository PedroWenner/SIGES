using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IConfiguracao
    {
        Task<List<ConfiguracaoModel>> ObterTodasConfiguracoesAsync();

        Task<string> ObterValorConfiguracaoAsync(string chave);

        Task AtualizarConfiguracaoAsync(string chave, string valor);
    }
}