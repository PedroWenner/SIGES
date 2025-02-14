using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SIGES.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGES.Models
{
    public class LancamentoModel
    {
        [IgnoreLog]
        [Key]
        [Column("idLancamentos")]
        public int Id { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("valor", TypeName = "decimal(15,2)")]
        public decimal Valor { get; set; } = 0;

        [Column("desconto", TypeName = "decimal(15,2)")]
        public decimal Desconto { get; set; } = 0;

        [Column("valor_desconto", TypeName = "decimal(15,2)")]
        public decimal ValorDesconto { get; set; } = 0;

        [Column("tipo_desconto")]
        public string? TipoDesconto { get; set; }

        [Column("data_vencimento")]
        public DateTime DataVencimento { get; set; }

        [Column("data_pagamento")]
        public DateTime? DataPagamento { get; set; }

        [Column("baixado")]
        public int Baixado { get; set; } = 0;

        [Column("cliente_fornecedor")]
        public string? ClienteFornecedor { get; set; }

        [Column("forma_pgto")]
        public string? FormaPgto { get; set; }

        [Column("tipo")]
        public string? Tipo { get; set; }

        [Column("anexo")]
        public string? AnexoBlob { get; set; }

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [IgnoreLog]
        [Column("clientes_id")]
        public int? ClientesId { get; set; }
        
        [IgnoreLog]
        [Column("categorias_id")]
        public int? CategoriasId { get; set; }
        
        [IgnoreLog]
        [Column("contas_id")]
        public int? ContasId { get; set; }

        [IgnoreLog]
        [Column("vendas_id")]
        public int? VendasId { get; set; }

        [IgnoreLog]
        [Column("usuario_id")]
        public int? UsuariosId { get; set; }

        [IgnoreLog]
        [Column("os_id")]
        public int? OsId { get; set; }
        
        public virtual ClienteModel? Clientes { get; set; }
        public virtual CategoriaModel? Categorias { get; set; }
        public virtual ContasModel? Contas { get; set; }
        public virtual VendaModel? Venda { get; set; }
        public virtual UsuarioModel? Usuarios { get; set; }
        public virtual OsModel? Os { get; set; }

        [ValidateNever]
        public ICollection<VendaModel> Vendas { get; set; } = new List<VendaModel>();
        [ValidateNever]
        public ICollection<OsModel> Ordem { get; set; } = new List<OsModel>();

    }
}
