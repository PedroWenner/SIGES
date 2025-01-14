using HtmlAgilityPack;
using System.Net;
using Microsoft.Security.Application;

namespace SIGES.Helpers
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Remove todas as tags HTML do conteúdo.
        /// </summary>
        public static string RemoverTagsHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Retorna o texto puro
            return doc.DocumentNode.InnerText;
        }

        /// <summary>
        /// Higieniza o HTML para remover scripts ou elementos perigosos.
        /// </summary>
        public static string HigienizarHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return Sanitizer.GetSafeHtmlFragment(html);
        }

        /// <summary>
        /// Decodifica entidades HTML para texto simples.
        /// </summary>
        public static string DecodificarHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            // Decodifica entidades HTML (&amp; -> &, etc.)
            return WebUtility.HtmlDecode(html);
        }
    }
}
