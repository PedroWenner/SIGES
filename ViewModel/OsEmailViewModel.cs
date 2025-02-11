using SIGES.Models;

public class OsEmailViewModel
{
    public OsModel? Os { get; set; }
    public EmitenteModel? Emitente { get; set; }
    public List<ProdutosOs?> Produtos { get; set; } = new List<ProdutosOs?>();
    public List<ServicosOs?> Servicos { get; set; } = new List<ServicosOs?>();
    public List<ArquivoModelOs?> Anexos { get; set; } = new List<ArquivoModelOs?>();
    public List<AnotacoesOs?> Anotacoes { get; set; } = new List<AnotacoesOs?>();
    public string PodeGerarPix { get; set; } = string.Empty;
    public string? Control2Vias { get; set; } = string.Empty;
}

public class ArquivoModelOs
{
    public string FileName { get; set; }
    public string Url { get; set; }
    public string FileType { get; set; }
}
