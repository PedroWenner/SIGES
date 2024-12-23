using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class VendaViewModel
    {
        public IList<VendaModel> Vendas { get; set; } = new List<VendaModel>();
        
        public IList<ItensVendaModel> Itens { get; set; } = new List<ItensVendaModel>();
        
        public VendaModel Venda { get; set; }
        
        public ItensVendaModel Item { get; set; }
    }
}