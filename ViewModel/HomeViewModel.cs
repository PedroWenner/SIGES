using SIGES.Models;

namespace SIGES.ViewModel
{
    public class HomeViewModel
    {
        public IList<ClienteModel> Clientes { get; set; } = new List<ClienteModel>();
        public IList<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
        public IList<PedidoModel> Pedidos { get; set; } = new List<PedidoModel>();
    }
}
