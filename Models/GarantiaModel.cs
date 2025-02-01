using SIGES.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SIGES.Models
{
    public class GarantiaModel
    {
        [IgnoreLog]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public DateTime? DataGarantia { get; set; }
        
        public string? RefGarantia { set; get; }
        
        [IgnoreLog]
        public string? TextoGarantia { get; set; }
        
        [IgnoreLog]
        public int? UsuarioId { get; set; }
        
        [IgnoreLog]
        [ValidateNever]
        public UsuarioModel Usuario { get; set; }

        [IgnoreLog]
        [ValidateNever]
        public ICollection<OsModel> Ordens { get; set; } = new List<OsModel>();

    }
}
