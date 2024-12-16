using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class ServicoViewModel
    {
        public IList<ServicoModel> Servicos { get; set; } = new List<ServicoModel>();
    }
}
