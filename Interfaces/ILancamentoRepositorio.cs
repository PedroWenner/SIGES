using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface ILancamentoRepositorio
    {
        Task Adicionar(LancamentoModel lancamento);

        Task<IList<LancamentoModel>> Listar();
        
        Task<LancamentoModel> Excluir(int id);

        Task Atualizar(LancamentoModel lancamento);
    }
}
