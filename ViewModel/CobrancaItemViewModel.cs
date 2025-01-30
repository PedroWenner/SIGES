using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class CobrancaItemViewModel
    {
        public int Id { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentMethod { get; set; }
        public string Total { get; set; }
        public string Link { get; set; }
        public string Barcode { get; set; }
        public DateTime ExpireAt { get; set; }
        public int? OsId { get; set; }
        public int? VendasId { get; set; }
        public string Status { get; set; }
        public string StatusDescricao { get; set; }
    }
}
