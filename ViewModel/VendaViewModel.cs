using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class VendaViewModel
    {
        public IList<VendaModel> Vendas { get; set; } = new List<VendaModel>();
        public VendaModel Venda { get; set; }
    }
}