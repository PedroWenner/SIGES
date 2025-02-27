using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class ClienteViewModel
    {
        public IList<ClienteModel> Clientes { get; set; } = new List<ClienteModel>();
        public List<OsModel> OrdensServicos { get; set; } = new List<OsModel>();
        public List<VendaModel> Vendas { get; set; } = new List<VendaModel>();
        public ClienteModel Cliente = new ClienteModel();
    }
}
