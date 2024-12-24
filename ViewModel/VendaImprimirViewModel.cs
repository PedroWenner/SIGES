using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class VendaImprimirViewModel
    {
        public VendaModel Venda { get; set; }
        public List<ItensVendaModel> ItensVenda { get; set; }
        public EmitenteModel Emitente { get; set; }
    }
}