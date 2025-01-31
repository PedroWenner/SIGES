using SIGES.Models;

namespace SIGES.ViewModel
{
    public class PaymentViewModel
    {
        public VendaModel? Venda { get; set; } = new VendaModel();
        public OsModel? Os { get; set; } = new OsModel();
        public string Tipo { get; set; }
        public string FormaPagamento { get; set; }
    }
}