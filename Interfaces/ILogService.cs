using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface ILogService
    {
        Task<List<LogAlteracaoModel>> ObterLogsPorEntidade(string entidade, int entidadeId);
        Task RegistrarAlteracoes<T>(string usuario, T original, T atualizado) where T : class;
        Task RegistrarAdicao<T>(string usuario, T entidade) where T : class;
        Task RegistrarExclusao<T>(string usuario, T entidade) where T : class;
    }
}
