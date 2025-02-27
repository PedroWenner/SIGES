using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IClienteRepositorio
    {
        IEnumerable<ClienteModel> Listar();
        Task<ClienteModel?> ObterPorId(int id);
        Task Adicionar(ClienteModel cliente);
        Task Atualizar(ClienteModel cliente);
        Task Excluir(int id);
        Task<bool> DocumentoExiste(string documento);
        IEnumerable<OsModel?> GetOs(int id);
        IEnumerable<VendaModel?> GetVendas(int id);
    }
}
