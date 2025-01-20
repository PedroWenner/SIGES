using Microsoft.AspNetCore.Mvc;
using SIGES.Services;

namespace SIGES.Views.Shared.Components.TemaPadrao
{
    public class TemaPadraoViewComponent : ViewComponent
    {
        private readonly TemaService _temaService;

        public TemaPadraoViewComponent(TemaService temaService)
        {
            _temaService = temaService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string appTheme = await _temaService.ObterTemaAsync();
            return View("Default", appTheme);
        }
    }
}
