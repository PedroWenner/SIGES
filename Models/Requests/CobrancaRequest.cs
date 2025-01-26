namespace SIGES.Models.Requests
{
    public class CobrancaRequest
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = string.Empty;
        public string GatewayDePagamento { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

    public class ExcluirCobrancaRequest
    {
        public int Id { get; set; }
    }

    public class AtualizarCobrancaRequest
    {
        public int Id { get; set; }
    }

    public class ConfirmarPagamentoRequest
    {
        public int Id { get; set; }
    }

    public class CancelarCobrancaRequest
    {
        public int Id { get; set; }
    }
}
