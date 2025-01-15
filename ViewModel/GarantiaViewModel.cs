using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class GarantiaViewModel
    {
        public IList<GarantiaModel> Garantias { get; set; } = new List<GarantiaModel>();

        public GarantiaModel Garantia = new GarantiaModel();
    }
}
