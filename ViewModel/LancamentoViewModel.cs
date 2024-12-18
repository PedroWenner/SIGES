using SIGES.Classes;
using SIGES.Models;

namespace SIGES.ViewModel
{
    public class LancamentoViewModel
    {
        public IList<LancamentoModel> Lancamentos { get; set; } = new List<LancamentoModel>();
        public LancamentoModel Lancamento { get; set; }
    }
}