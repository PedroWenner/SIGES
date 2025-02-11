using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Controllers.Api;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Services;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("os")]
    public class OrdemController : Controller
    {
        private readonly IOs _osRepositorio;
        private readonly ILogService _logServiceRepositorio;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;
        private readonly IConfiguracao _configuracaoRepositorio;
        private readonly IRepository<ProdutoModel> _repositorioProdutos;
        private readonly IRepository<ProdutosOs> _repositorioProdutosOs;
        private readonly IRepository<ServicosOs> _repositorioServicosOs;
        private readonly IRepository<ServicoModel> _repositorioServico;
        private readonly IRepository<AnotacoesOs> _repositorioAnotacao;
        private readonly PaymentGatewaysService _paymentGatewaysService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OrdemController(IOs osRepositorio, ILogService logServiceRepositorio,
            IRepository<EmitenteModel> repositorioEmitente, IConfiguracao configuracaoRepositorio,
            IRepository<ProdutosOs> repositorioProdutosOs, IRepository<ServicosOs> repositorioServicosOs,
            IRepository<ProdutoModel> repositorioProdutos, IRepository<ServicoModel> repositorioServico,
            IRepository<AnotacoesOs> repositorioAnotacao, PaymentGatewaysService paymentGatewaysService,
            HttpClient httpClient, IConfiguration configuration)
        {
            _osRepositorio = osRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
            _repositorioEmitente = repositorioEmitente;
            _configuracaoRepositorio = configuracaoRepositorio;
            _repositorioProdutosOs = repositorioProdutosOs;
            _repositorioServicosOs = repositorioServicosOs;
            _repositorioProdutos = repositorioProdutos;
            _repositorioServico = repositorioServico;
            _repositorioAnotacao = repositorioAnotacao;
            _paymentGatewaysService = paymentGatewaysService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var ordens = _osRepositorio.Listar();
            var viewModel = new OsViewModel
            {
                Ordens = ordens.ToList(),
            };

            return View("LOrdem", viewModel);
        }

        [Permissao("aOs", "Você não tem permissão para adicionar OS.")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AOrdem");
        }

        [Permissao("aOs", "Você não tem permissão para adicionar OS.")]
        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(OsModel os)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Favor validar todos os campos obrigatórios." });
            }

            os.faturado = 0;

            try
            {
                await _osRepositorio.Adicionar(os);

                var emitente = await _repositorioEmitente.GetLastRegister();
                var osCadastrada = await _osRepositorio.ObterPorId(os.Id);

                var emailResponse = await EnviarEmailAsync(osCadastrada.Id, "Ordem de serviço - criada", "os", "Email/Os/OsEmail", "S");
                //ele vai ser chamado aqui passando o oscadastrada.id como parametro

                return Json(new { status = "success", message = "OS adicionada com sucesso, você pode adicionar produtos ou serviços a essa OS nas abas de produtos e serviços!", id = os.Id.ToString() });
            }
            catch(Exception)
            {
                return Json(new { status = "error", message = "Ocorreu um erro." });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca pelo ID
            var os = await _osRepositorio.ObterPorId(id);
            if (os == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            var emitente = await _repositorioEmitente.GetLastRegister();
            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);
            var anotacoes = _osRepositorio.GetAnotacaoes(id);
            
            // Defina o caminho onde os anexos dessa OS estão armazenados
            string pastaAnexos = Path.Combine("wwwroot/anexos", $"OS-{id}");
            List<ArquivoModelOs> anexos = new List<ArquivoModelOs>();

            if (Directory.Exists(pastaAnexos))
            {
                var arquivos = Directory.GetFiles(pastaAnexos);
                foreach (var arquivo in arquivos)
                {
                    string fileName = Path.GetFileName(arquivo);
                    string fileUrl = $"/anexos/OS-{id}/{fileName}";

                    anexos.Add(new ArquivoModelOs
                    {
                        FileName = fileName,
                        Url = fileUrl,
                        FileType = ObterTipoArquivo(fileName)
                    });
                }
            }

            var viewmodel = new OsEmailViewModel
            {
                Os = os,
                Produtos = produtos.ToList(),
                Servicos = servicos.ToList(),
                Anotacoes = anotacoes.ToList(),
                Anexos = anexos,
                Emitente = emitente
            };

            return View("EOrdem", viewmodel);
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, OsModel os)
        {
            if (!ModelState.IsValid)
            {
                return View("EOrdem", os);
            }

            // Busca a categoria existente
            var osExistente = await _osRepositorio.ObterPorId(id);

            if (osExistente == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, osExistente, os);

            // Salva as alterações
            osExistente.Status = os.Status;
            osExistente.ClientesId = os.ClientesId;
            osExistente.UsuariosId = os.UsuariosId;
            osExistente.DataInicial = os.DataInicial;
            osExistente.DataFinal = os.DataFinal;
            osExistente.Garantia = os.Garantia;
            osExistente.GarantiasId = os.GarantiasId;
            osExistente.DescricaoProduto = os.DescricaoProduto;
            osExistente.Defeito = os.Defeito;
            osExistente.Observacoes = os.Observacoes;
            osExistente.LaudoTecnico = os.LaudoTecnico;
            await _osRepositorio.Atualizar(osExistente);

            TempData["Success"] = "OS atualizada com sucesso!";
            return RedirectToAction("Editar", new { id = osExistente.Id });
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            // Busca a categoria pelo ID
            var os = await _osRepositorio.ObterPorId(id);

            if (os == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, os);
            await _osRepositorio.Excluir(id);

            TempData["Success"] = "OS excluída com sucesso!";

            return RedirectToAction("Index");
        }

        [Permissao("vOs", "Você não possui permissão para visualizar OS.")]
        [HttpGet("visualizar/{id}")]
        public async Task<IActionResult> Visualizar(int id)
        {
            var os = await _osRepositorio.ObterPorId(id);
            if (os == null)
            {
                TempData["Error"] = "OS não encontrada.";
                return RedirectToAction("Index");
            }

            var emitente = await _repositorioEmitente.GetLastRegister();
            var produtos = _osRepositorio.GetProdutos(id);
            var servicos = _osRepositorio.GetServicos(id);
            var anotacoes = _osRepositorio.GetAnotacaoes(id);

            // Defina o caminho onde os anexos dessa OS estão armazenados
            string pastaAnexos = Path.Combine("wwwroot/anexos", $"OS-{id}");
            List<ArquivoModelOs> anexos = new List<ArquivoModelOs>();

            if (Directory.Exists(pastaAnexos))
            {
                var arquivos = Directory.GetFiles(pastaAnexos);
                foreach (var arquivo in arquivos)
                {
                    string fileName = Path.GetFileName(arquivo);
                    string fileUrl = $"/anexos/OS-{id}/{fileName}";

                    anexos.Add(new ArquivoModelOs
                    {
                        FileName = fileName,
                        Url = fileUrl,
                        FileType = ObterTipoArquivo(fileName)
                    });
                }
            }

            var podeGerarPix = _paymentGatewaysService.GetGateways()["MercadoPago"].Credentials["access_token"].ToString();

            decimal? valorTotal = produtos.Sum(i => i.SubTotal) + servicos.Sum(i => i.SubTotal ?? 0);
            var viewmodel = new OsEmailViewModel
            {
                Os = os,
                Produtos = produtos.ToList(),
                Servicos = servicos.ToList(),
                Anotacoes = anotacoes.ToList(),
                Emitente = emitente,
                PodeGerarPix = podeGerarPix,
                Anexos = anexos
            };

            return View("VOs", viewmodel);
        }

        private async Task<object> EnviarEmailAsync(int id, string assunto, string tipo, string nomeview, string automaticoMail)
        {
            try
            {
                EmailRequest email = new EmailRequest
                {
                    Assunto = assunto,
                    Tipo = tipo,
                    Nomeview = nomeview,
                    AutomaticoMail = automaticoMail
                };
                var apiUrl = $"{_configuration["ApiUrls:Mensagem"]}/enviar-email/{id}?email={email}";

                var response = await _httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<object>();
                }

                return new { status = "error", message = "Falha ao enviar e-mail." };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("adicionar-produto")]
        public async Task<IActionResult> AdicionarProdutos(ProdutosOs produtoOs)
        {
            var os = await _osRepositorio.ObterPorId(produtoOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            if (os.faturado == 1)
            {
                return Json(new { status = "error", message = "Não é possivel adicionar produtos. OS já faturada!" });
            }

            produtoOs.SubTotal = (produtoOs.Preco ?? 0) * (produtoOs.Quantidade);

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Validar campos antes de continuar." });
            }

            var produto = await _repositorioProdutos.ObterPorId(produtoOs.ProdutosId);
            if (produto == null)
            {
                return Json(new { status = "error", message = "Produto não encontrado." });
            }

            var controlEstoque = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("control_estoque");
            if (controlEstoque == "1")
            {
                if (produtoOs.Quantidade > produto.Estoque)
                {
                    return Json(new { status = "error", message = "A quantidade não pode ser maior que o estoque disponível." });
                }
            }

            try
            {
                await _repositorioProdutosOs.Adicionar(produtoOs);

                if (controlEstoque == "1")
                {
                    produto.Estoque = produto.Estoque - produtoOs.Quantidade;
                    await _repositorioProdutos.Atualizar(produto);
                }

                os.Desconto = 0;
                os.ValorDesconto = 0;
                await _osRepositorio.Atualizar(os);

                var produtosAdicionados = _osRepositorio.GetProdutos(os.Id);

                return Json(new 
                {
                    status = "success",
                    produtos = produtosAdicionados.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produtos?.Descricao,
                        i.Quantidade,
                        Preco = i.Preco?.ToString("C"),
                        SubTotal = i.SubTotal.ToString("C")
                    }),
                    total = produtosAdicionados.Sum(i => i.SubTotal).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("remover-produto")]
        public async Task<IActionResult> RemoverProduto(int produtoId)
        {
            var produtoOs = await _repositorioProdutosOs.ObterPorId(produtoId);
            if (produtoOs == null)
            {
                return Json(new { status = "error", message = "Produto não encontrado." });
            }

            var os = await _osRepositorio.ObterPorId(produtoOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            if (os.faturado == 1)
            {
                return Json(new { status = "error", message = "Não é possivel excluir produtos. OS já faturada!" });
            }

            var produto = await _repositorioProdutos.ObterPorId(produtoOs.ProdutosId);
            if (produto == null)
            {
                return Json(new { status = "error", message = "Produto não encontrado." });
            }

            var controlEstoque = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("control_estoque");
            int quantidade = produtoOs.Quantidade;
            try
            {
                await _repositorioProdutosOs.Deletar(produtoOs);

                if (controlEstoque == "1")
                {
                    produto.Estoque = produto.Estoque + quantidade;
                    await _repositorioProdutos.Atualizar(produto);
                }

                os.Desconto = 0;
                os.ValorDesconto = 0;
                await _osRepositorio.Atualizar(os);

                var produtosAdicionados = _osRepositorio.GetProdutos(os.Id);

                return Json(new 
                {
                    status = "success",
                    produtos = produtosAdicionados.Select(i => new
                    {
                        i.Id,
                        ProdutoNome = i.Produtos?.Descricao,
                        i.Quantidade,
                        Preco = i.Preco?.ToString("C"),
                        SubTotal = i.SubTotal.ToString("C")
                    }),
                    total = produtosAdicionados.Sum(i => i.SubTotal).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("adicionar-servico")]
        public async Task<IActionResult> AdicionarServico(ServicosOs servicosOs)
        {
            var os = await _osRepositorio.ObterPorId(servicosOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            if (os.faturado == 1)
            {
                return Json(new { status = "error", message = "Não é possivel adicionar produtos. OS já faturada!" });
            }

            servicosOs.SubTotal = (servicosOs.Preco ?? 0) * (servicosOs.Quantidade);

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Validar campos antes de continuar." });
            }

            var servico = await _repositorioServico.ObterPorId(servicosOs.ServicosId);
            if (servico == null)
            {
                return Json(new { status = "error", message = "Serviço não encontrado." });
            }

            try
            {
                await _repositorioServicosOs.Adicionar(servicosOs);

                os.Desconto = 0;
                os.ValorDesconto = 0;
                await _osRepositorio.Atualizar(os);

                var servicosAdicionados = _osRepositorio.GetServicos(os.Id);

                return Json(new 
                {
                    status = "success",
                    servicos = servicosAdicionados.Select(i => new
                    {
                        i.Id,
                        ServicoNome = i.Servicos?.Nome,
                        i.Quantidade,
                        Preco = i.Preco?.ToString("C"),
                        SubTotal = i.SubTotal?.ToString("C")
                    }),
                    total = servicosAdicionados.Sum(i => i.SubTotal ?? 0).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("remover-servico")]
        public async Task<IActionResult> RemoverServico(int servicoId)
        {
            var servicosOs = await _repositorioServicosOs.ObterPorId(servicoId);
            if (servicosOs == null)
            {
                return Json(new { status = "error", message = "Serviço não encontrado." });
            }

            var os = await _osRepositorio.ObterPorId(servicosOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            if (os.faturado == 1)
            {
                return Json(new { status = "error", message = "Não é possivel excluir produtos. OS já faturada!" });
            }

            var servico = await _repositorioProdutos.ObterPorId(servicosOs.ServicosId);
            if (servico == null)
            {
                return Json(new { status = "error", message = "Serviço não encontrado." });
            }

            try
            {
                await _repositorioServicosOs.Deletar(servicosOs);

                os.Desconto = 0;
                os.ValorDesconto = 0;
                await _osRepositorio.Atualizar(os);

                var servicosAdicionados = _osRepositorio.GetServicos(os.Id);

                return Json(new
                {
                    status = "success",
                    servicos = servicosAdicionados.Select(i => new
                    {
                        i.Id,
                        ServicoNome = i.Servicos?.Nome,
                        i.Quantidade,
                        Preco = i.Preco?.ToString("C"),
                        SubTotal = i.SubTotal?.ToString("C")
                    }),
                    total = servicosAdicionados.Sum(i => i.SubTotal ?? 0).ToString("C")
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("anexar")]
        public async Task<IActionResult> Anexar(int osId)
        {
            var files = Request.Form.Files;

            if (files == null || files.Count == 0)
            {
                return Json(new { status = "error", message = "Nenhum arquivo enviado." });
            }

            var directory = Path.Combine("wwwroot", "anexos", $"OS-{osId}");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!Directory.Exists(Path.Combine(directory, "thumbs")))
                Directory.CreateDirectory(Path.Combine(directory, "thumbs"));

            List<string> arquivosSalvos = new List<string>();

            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (!AllowedExtensions(extension))
                        {
                            return Json(new { status = "error", message = "Extensão não permitida." });
                        }
                        
                        var newFileName = $"{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(directory, newFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var isImage = file.ContentType.StartsWith("image");
                        string thumbPath = "";

                        if (isImage)
                        {
                            thumbPath = Path.Combine(directory, "thumbs", $"thumb_{newFileName}");
                            CriarThumbnail(filePath, thumbPath);
                        }

                        arquivosSalvos.Add($"/anexos/OS-{osId}/{newFileName}");
                    }
                }

                return Json(new { status = "success", message = "Arquivo(s) anexado(s) com sucesso.", arquivos = arquivosSalvos });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = $"Erro ao salvar arquivo(s): {ex.Message}" });
            }
        }

        private void CriarThumbnail(string inputPath, string outputPath)
        {
            using var image = System.Drawing.Image.FromFile(inputPath);
            var thumbnail = image.GetThumbnailImage(200, 125, () => false, IntPtr.Zero);
            thumbnail.Save(outputPath);
        }

        // Método para determinar o tipo do arquivo (imagem, PDF, etc.)
        private string ObterTipoArquivo(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
            {
                return "image";
            }
            return "file";
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpGet("listar-anexos/{osId}")]
        public IActionResult ListarAnexos(int osId)
        {
            var directory = Path.Combine("wwwroot", "anexos", $"OS-{osId}");

            if (!Directory.Exists(directory))
            {
                return Json(new { status = "success", arquivos = new List<string>() });
            }

            var arquivos = Directory.GetFiles(directory)
                .Where(file => !file.Contains("thumb_"))
                .Select(file => $"/anexos/OS-{osId}/{Path.GetFileName(file)}")
                .ToList();

            return Json(new { status = "success", arquivos = arquivos });
        }

        private bool AllowedExtensions(string extensao)
        {
            List<string> extensoesPermitidas = new List<string> { ".jpg", ".png", ".gif", ".jpeg", ".JPG", ".PNG", ".GIF", ".JPEG", ".pdf", ".PDF", ".cdr", ".CDR", ".docx", ".DOCX", ".txt" };
            if (extensoesPermitidas.Contains(extensao))
            {
                return true;
            }

            return false;
        }

        [Permissao("eOs", "Você não tem permissão para baixar anexos")]
        [HttpGet("download/{filename}")]
        public IActionResult Download(string fileName)
        {
            var directory = Path.Combine("wwwroot", "anexos");
            var filePath = Directory.GetFiles(directory, fileName, SearchOption.AllDirectories).FirstOrDefault();

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/octet-stream";
            return File(fileBytes, contentType, fileName);
        }

        [Permissao("eOs", "Você não tem permissão para excluir anexos")]
        [HttpPost("excluir-anexo/{filename}")]
        public IActionResult ExcluirAnexo(string fileName)
        {
            try
            {
                var directory = Path.Combine("wwwroot", "anexos");
                var filePath = Directory.GetFiles(directory, fileName, SearchOption.AllDirectories).FirstOrDefault();

                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                    return Json(new { status = "error", message = "Arquivo não encontrado." });

                System.IO.File.Delete(filePath);

                var thumbPath = Path.Combine(Path.GetDirectoryName(filePath)!, "thumbs", $"thumb_{fileName}");
                if (System.IO.File.Exists(thumbPath))
                {
                    System.IO.File.Delete(thumbPath);
                }

                return Json(new { status = "success", message = "Arquivo excluído com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = $"Erro ao excluir arquivo: {ex.Message}" });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("adicionar-anotacao")]
        public async Task<IActionResult> AdicionarAnotacao(AnotacoesOs anotacoesOs)
        {
            var os = await _osRepositorio.ObterPorId(anotacoesOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Validar campos antes de continuar." });
            }

            try
            {
                anotacoesOs.Anotacao = HtmlHelpers.HigienizarHtml(anotacoesOs.Anotacao);
                anotacoesOs.Anotacao = "[" + User?.Identity?.Name + "] " + anotacoesOs.Anotacao;
                await _repositorioAnotacao.Adicionar(anotacoesOs);
                var anotacoesAdicionados = _osRepositorio.GetAnotacaoes(os.Id);

                return Json(new
                {
                    status = "success",
                    anotacoes = anotacoesAdicionados.Select(i => new
                    {
                        i.Id,
                        i.Anotacao,
                        DataHora = i.DataHora.ToString("dd/MM/yyyy HH:mm:ss")
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("excluir-anotacao/{id}")]
        public async Task<IActionResult> ExcluirAnotacao(int id)
        {
            var anotacoesOs = await _repositorioAnotacao.ObterPorId(id);
            if (anotacoesOs == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            var os = await _osRepositorio.ObterPorId(anotacoesOs.OsId);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            try
            {
                await _repositorioAnotacao.Deletar(anotacoesOs);
                var anotacoesAdicionados = _osRepositorio.GetAnotacaoes(os.Id);

                return Json(new
                {
                    status = "success",
                    anotacoes = anotacoesAdicionados.Select(i => new
                    {
                        i.Id,
                        i.Anotacao,
                        DataHora = i.DataHora.ToString("dd/MM/yyyy HH:mm:ss")
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [Permissao("eOs", "Você não tem permissão para editar OS.")]
        [HttpPost("aplicar-desconto")]
        public async Task<IActionResult> AplicarDesconto(OsModel os)
        {
            var osExistente = await _osRepositorio.ObterPorId(os.Id);
            if (os == null)
            {
                return Json(new { status = "error", message = "OS não encontrada." });
            }

            string status = osExistente?.Status ?? "";
            if (osExistente?.faturado == 1 || status == "Cancelado" || status == "Faturado")
            {
                return Json(new { status = "error", message = "Não é possivel aplicar desconto. OS já faturada/cancelada!" });
            }

            if (os.Desconto == 0)
            {
                return Json(new { status = "error", message = "Valor/Percentual do desconto não informado!" });
            }

            var produtos = _osRepositorio.GetProdutos(os.Id);
            var servicos = _osRepositorio.GetServicos(os.Id);

            decimal valorProdutos = produtos == null ? 0 : (produtos.Sum(x => x.SubTotal));
            decimal valorServicos = servicos == null ? 0 : servicos.Sum(x => x.SubTotal ?? 0);
            decimal valorTotal = valorProdutos + valorServicos;

            if (valorTotal == 0)
            {
                return Json(new { status = "error", message = "Valor/Percentual do desconto não informado!" });
            }

            osExistente.Desconto = os.Desconto;
            osExistente.TipoDesconto = os.TipoDesconto;
            osExistente.ValorDesconto = CalcularDesconto(os.TipoDesconto, os.Desconto, valorTotal);

            try
            {
                await _osRepositorio.Atualizar(osExistente);

                return Json(new
                {
                    status = "success",
                    message = "Desconto aplicado com sucesso!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        private decimal CalcularDesconto(string tipoDesconto, decimal desconto, decimal valorTotal)
        {
            decimal descontoDef = 0;
            if (tipoDesconto == "porcento")
            {
                descontoDef = valorTotal * desconto / 100;
            }
            else if (tipoDesconto == "real")
            {
                descontoDef = desconto;
            }
            else
            {
                descontoDef = 0;
            }

            return valorTotal - descontoDef;
        }
    }
}