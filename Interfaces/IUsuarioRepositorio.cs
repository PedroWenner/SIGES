using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IUsuarioRepositorio
    {
        IEnumerable<UsuarioModel> Listar();
        Task<UsuarioModel?> ObterPorId(int id);
        Task Adicionar(UsuarioModel usuario);
        Task Atualizar(UsuarioModel usuario);
        Task Excluir(int id);
        Task<bool> EmailExiste(string email);
        Task<bool> DocumentoExiste(string documento);
    }
}
