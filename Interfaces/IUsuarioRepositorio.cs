using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IUsuarioRepositorio
    {
        IQueryable<UsuarioModel> Listar();
        
        Task<UsuarioModel?> ObterPorId(int id);
        
        Task Adicionar(UsuarioModel usuario);
        
        Task Atualizar(UsuarioModel usuario);
        
        Task Excluir(int id);
    }
}
