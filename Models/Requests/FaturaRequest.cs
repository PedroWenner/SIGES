namespace SIGES.Models.Requests
{
    public class FaturaRequest
    {
        public int? VendaId { get; set; }
        public int? OsId { get; set; }
        public string Grupo { get; set; }
        public string Descricao { get; set; }
        public int ClienteId { get; set; }
        public string Tipo { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public DateTime DataVencimento { get; set; }
        public int Recebido { get; set; } = 0;
        public DateTime? DataRecebimento { get; set; }
        public string? FormaPgto { get; set; }
    }
}
