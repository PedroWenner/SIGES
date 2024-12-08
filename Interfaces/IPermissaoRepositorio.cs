using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IPermissaoRepositorio
    {
        IQueryable<PermissaoModel> Listar();

        Task<PermissaoModel?> ObterPorId(int id);

        Task Adicionar(PermissaoModel permissao);

        Task Atualizar(PermissaoModel permissao);

        Task Excluir(int id);
    }
}
