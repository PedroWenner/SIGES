namespace SIGES.Models.Requests
{
    public class FaturarOsRequest
    {
        public int OsId { get; set; }
        public string Descricao { get; set; }
        public int ClienteId { get; set; }
        public decimal Valor { get; set; }
        public decimal FaturarDesconto { get; set; }
        public DateTime DataVencimento { get; set; }
        public int Recebido { get; set; }
        public DateTime? Recebimento { get; set; }
        public string? FormaPgto { get; set; }
        public string Tipo { get; set; }
    }

}
