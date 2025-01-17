using HtmlAgilityPack;
using System.Net;
using Microsoft.Security.Application;
using System.Text;

namespace SIGES.Helpers
{
    public static class HtmlHelpers
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

        /// <summary>
        /// Realiza a quebra de string
        /// </summary>
        /// <returns></returns>
        public static string TruncateString(string input, int tamanho, string ellipsis = "...")
        {
            if (string.IsNullOrEmpty(input) || tamanho <= 0) return string.Empty;

            int byteCount = 0;
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                int charByteSize = Encoding.UTF8.GetByteCount(c.ToString());

                if (byteCount + charByteSize > tamanho)
                {
                    return result.ToString() + ellipsis;
                }

                result.Append(c);
                byteCount += charByteSize;
            }

            return result.ToString();
        }
    }
}
