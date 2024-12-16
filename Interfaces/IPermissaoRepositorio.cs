using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IPermissaoRepositorio
    {
        IEnumerable<PermissaoModel> Listar();

        Task<PermissaoModel?> ObterPorId(int id);

        Task Adicionar(PermissaoModel permissao);

        Task Atualizar(PermissaoModel permissao);

        Task Excluir(int id);

        // Métodos relacionados a permissões do usuário
        Task<List<PermissaoDetalheModel>> ObterPermissoesUsuario(int userId); // Obtém permissões de um usuário específico
        Task<bool> VerificarPermissao(int userId, string permissao); // Verifica se o usuário possui uma permissão específica
    }
}
