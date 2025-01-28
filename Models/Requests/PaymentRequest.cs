namespace SIGES.Models.Requests
{
    public class PaymentRequest
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal? TransactionAmount { get; set; }
        public string? Description { get; set; }
        public string? PaymentMethodId { get; set; } = string.Empty;
        public string? Identification { get; set; } = string.Empty;
        public string? ZipCode { get; set; } = string.Empty;
        public string? StreetName { get; set; } = string.Empty;
        public string? StreetNumber { get; set; } = string.Empty;
        public string? Neighborhood { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? FederalUnit { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public int? ClienteId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
