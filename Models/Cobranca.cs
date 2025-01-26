using SIGES.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGES.Models
{
    [Table("cobrancas")]
    public class Cobranca
    {
        [Key]
        [Column("idCobranca")]
        public int Id { get; set; }

        [Column("charge_id")]
        [MaxLength(255)]
        public string? ChargeId { get; set; }

        [Column("conditional_discount_date")]
        public DateTime? ConditionalDiscountDate { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("custom_id")]
        public int? CustomId { get; set; }

        [Column("expire_at")]
        [Required]
        public DateTime ExpireAt { get; set; }

        [Column("message")]
        [Required]
        [MaxLength(255)]
        public string Message { get; set; } = string.Empty;

        [Column("payment_method")]
        [MaxLength(11)]
        public string? PaymentMethod { get; set; }

        [Column("payment_url")]
        [MaxLength(255)]
        public string? PaymentUrl { get; set; }

        [Column("request_delivery_address")]
        [MaxLength(64)]
        public string? RequestDeliveryAddress { get; set; }

        [Column("status")]
        [Required]
        [MaxLength(36)]
        public string Status { get; set; } = "Aguardando pagamento";

        [Column("total")]
        [MaxLength(15)]
        public string? Total { get; set; }

        [Column("barcode")]
        [Required]
        [MaxLength(255)]
        public string Barcode { get; set; } = string.Empty;

        [Column("link")]
        [Required]
        [MaxLength(255)]
        public string Link { get; set; } = string.Empty;

        [Column("payment_gateway")]
        [MaxLength(255)]
        public string? PaymentGateway { get; set; }

        [Column("payment")]
        [Required]
        [MaxLength(64)]
        public string Payment { get; set; } = string.Empty;

        [Column("pdf")]
        [MaxLength(255)]
        public string? Pdf { get; set; }

        [Column("vendas_id")]
        public int? VendasId { get; set; }

        [Column("os_id")]
        public int? OsId { get; set; }

        [Column("clientes_id")]
        public int? ClientesId { get; set; }

        // Relacionamentos com outras tabelas (Opcional)
        public virtual VendaModel? Venda { get; set; }
        public virtual OsModel? Os { get; set; }
        public virtual ClienteModel? Cliente { get; set; }
    }
}
