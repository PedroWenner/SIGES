using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class OsViewModel
    {
        public IList<OsModel> Ordens { get; set; } = new List<OsModel>();
    }
}
