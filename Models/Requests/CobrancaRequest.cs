namespace SIGES.Models.Requests
{
    public class CobrancaRequest
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = string.Empty;
        public string GatewayDePagamento { get; set; } = string.Empty;
        public decimal? Valor { get; set; }
        public string? EmailCliente {  get; set; } = string.Empty;
        public int? ClienteId { get; set; }
        public virtual ClienteModel? Cliente { get; set; }
    }
}
