using Microsoft.AspNetCore.Mvc;
using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewComponents
{
    public class PaginacaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Paginacao<UsuarioModel> paginacao)
        {
            return View("Default", paginacao);
        }
    }
}
