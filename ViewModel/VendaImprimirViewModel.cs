using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class VendaImprimirViewModel
    {
        public VendaModel Venda { get; set; }
        public List<ItensVendaModel> ItensVenda { get; set; }
        public EmitenteModel Emitente { get; set; }
        public string QrCodeBase64 { get; set; } = string.Empty;
        public string ChavePixFormatada { get; set; } = string.Empty;
    }
}