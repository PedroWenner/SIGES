using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class PermissaoViewModel
    {
        public IList<PermissaoModel> Permissoes { get; set; } = new List<PermissaoModel>();
    }
}
