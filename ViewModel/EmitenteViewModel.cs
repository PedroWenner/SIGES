using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class EmitenteViewModel
    {
        public IList<EmitenteModel> Emitentes { get; set; } = new List<EmitenteModel>();

        public EmitenteModel Emitente = new EmitenteModel();
    }
}
