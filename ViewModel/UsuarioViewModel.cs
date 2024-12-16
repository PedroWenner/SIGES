using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class UsuarioViewModel
    {
        public IList<UsuarioModel> Usuarios { get; set; } = new List<UsuarioModel>();
        public IList<PermissaoModel> Permissoes { get; set; } = new List<PermissaoModel>();
        public UsuarioModel Usuario { get; set; }
    }
}