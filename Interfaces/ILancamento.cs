using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface ILancamento
    {
        Task Adicionar(LancamentoModel lancamento);
        IEnumerable<LancamentoModel> Listar();
        Task<LancamentoModel?> ObterPorId(int id);
        Task Atualizar(LancamentoModel lancamento);
        Task Excluir(int id);

        Task<IEnumerable<LancamentoModel>> GetVendasOs();
    }
}