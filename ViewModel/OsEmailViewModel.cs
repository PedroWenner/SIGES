using SIGES.Models;

public class OsEmailViewModel
{
    public OsModel Os { get; set; }
    public EmitenteModel Emitente { get; set; }
    public List<ProdutosOs?> Produtos { get; set; } = new List<ProdutosOs?>();
    public List<ServicosOs?> Servicos { get; set; } = new List<ServicosOs?>();
}