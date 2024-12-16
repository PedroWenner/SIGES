using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class ClienteViewModel
    {
        public IList<ClienteModel> Clientes { get; set; } = new List<ClienteModel>();
    }
}
