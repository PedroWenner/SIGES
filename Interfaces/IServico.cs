using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IServico
    {
        IEnumerable<ServicoModel> Listar();

        Task<ServicoModel?> ObterPorId(int id);

        Task Adicionar(ServicoModel servico);

        Task Atualizar(ServicoModel servico);

        Task Excluir(int id);
    }
}
