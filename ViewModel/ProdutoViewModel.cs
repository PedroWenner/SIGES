using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class ProdutoViewModel
    {
        public IList<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
    }
}
