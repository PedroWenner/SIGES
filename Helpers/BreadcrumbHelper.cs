using Microsoft.AspNetCore.Mvc.Rendering;
using SIGES.Classes;
using System.Text;

namespace SIGES.Helpers
{
    public class BreadcrumbHelper
    {
        public static string GerarBreadcrumb(ViewContext viewContext)
        {
            var path = viewContext.HttpContext.Request.Path.Value;

            // Divide o caminho da URL em segmentos
            var segmentos = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segmentos.Length == 0)
                return string.Empty; // Sem breadcrumb se não houver caminho

            var baseUrl = "/";
            var breadcrumbHtml = new StringBuilder();

            // Determina o último segmento válido (ignorando IDs ou números)
            var ultimoSegmento = Array.FindLastIndex(segmentos, segmento => !int.TryParse(segmento, out _));

            for (int i = 0; i < segmentos.Length; i++)
            {
                var segmento = segmentos[i];
                
                // Ignora segmentos que são IDs (números)
                if (int.TryParse(segmento, out _))
                {
                    continue;
                }

                baseUrl += segmento + "/";

                var ultimo = (i == ultimoSegmento);
                var cssClass = ultimo ? "current text-decoration-none fw-bold" : "text-decoration-none";

                // Adiciona o link ou o texto simples (último segmento)
                if (ultimo)
                {
                    breadcrumbHtml.Append($"<a class='active {cssClass}' aria-current='page' disabled>{segmento.Capitalize()}</a>");
                }
                else
                {
                    breadcrumbHtml.Append($"<a href='{baseUrl}' class='{cssClass}'>{segmento.Capitalize()}</a>");
                }
            }

            return breadcrumbHtml.ToString();
        }
    }
}
