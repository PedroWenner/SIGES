using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class CategoriaViewModel
    {
        public IList<CategoriaModel> Categorias { get; set; } = new List<CategoriaModel>();
    }
}
