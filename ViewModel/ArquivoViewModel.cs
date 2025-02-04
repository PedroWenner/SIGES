using SIGES.Models;

namespace SIGES.ViewModel
{
    public class ArquivoViewModel
    {
        public IList<ArquivoModel> Arquivos { get; set; } = new List<ArquivoModel>();
    }
}