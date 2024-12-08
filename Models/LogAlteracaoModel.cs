namespace SIGES.Models
{
    public class LogAlteracaoModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; } // Usuário que fez a alteração
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow; // Data da alteração
        public string Entidade { get; set; } // Nome da entidade alterada
        public int EntidadeId { get; set; } // ID da entidade alterada
        public string Alteracoes { get; set; } // JSON com as alterações
        public string TipoOperacao { get; set; }
    }
}
