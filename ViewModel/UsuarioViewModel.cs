using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class UsuarioViewModel
    {
        public Paginacao<UsuarioModel> Usuarios { get; set; }
        public IList<PermissaoModel> Permissoes { get; set; } = new List<PermissaoModel>();
        public UsuarioModel Usuario { get; set; }
    }
}