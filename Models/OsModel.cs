using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SIGES.Models
{
    public class OsModel
    {
        [IgnoreLog]
        [Column("id_os")]
        [Key]
        public int Id { get; set; }
        
        [Column("data_inicial")]
        public DateTime? DataInicial { get; set; }
        
        [Column("data_final")]
        public DateTime? DataFinal { get; set; }
        
        [Column("garantia")]
        public string? Garantia { get; set; }
        
        [Column("descricao_produto")]
        public string? DescricaoProduto { get; set; }
        
        [Column("defeito")]
        public string? Defeito { get; set; }
        
        [Column("status")]
        public string? Status { get; set; }

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [Column("laudo_tecnico")]
        public string? LaudoTecnico { get; set; }

        [Column("valor_total", TypeName = "decimal(15,2)")]
        public decimal ValorTotal { get; set; } = 0;

        [Column("desconto", TypeName = "decimal(15,2)")]
        public decimal Desconto { get; set; } = 0;

        [Column("valor_desconto", TypeName = "decimal(15,2)")]
        public decimal ValorDesconto { get; set; } = 0;

        [Column("tipo_desconto")]
        public string? TipoDesconto { get; set; }

        [Column("clientes_id")]
        public int ClientesId { get; set; }

        [Column("usuarios_id")]
        public int UsuariosId { get; set; }

        [Column("lancamento_id")]
        public int? LancamentoId { get; set; }

        [Column("faturado")]
        public int? faturado { get; set; } = 0;

        [Column("garantias_id")]
        public int? GarantiasId { get; set; }

        public virtual ClienteModel? Clientes { get; set; }
        public virtual UsuarioModel? Usuarios { get; set; }
        public virtual GarantiaModel? Garantias { get; set; }
        public virtual LancamentoModel? Lancamento { get; set; }

        [ValidateNever]
        public List<ServicosOs> Servicos { get; set; } = new List<ServicosOs>();

        [ValidateNever]
        public List<ProdutosOs> Produtos { get; set; } = new List<ProdutosOs>();

        [ValidateNever]
        public List<AnotacoesOs> Anotacoes { get; set; } = new List<AnotacoesOs>();
        
        [ValidateNever]
        public List<LancamentoModel> Lancamentos { get; set; } = new List<LancamentoModel>();

    }
}
