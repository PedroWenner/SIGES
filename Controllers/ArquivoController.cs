using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("arquivos")]
    public class ArquivoController : Controller
    {
        private readonly IRepository<ArquivoModel> _repositorio;

        public ArquivoController(IRepository<ArquivoModel> repositorio)
        {
            _repositorio = repositorio;
        }

        [Permissao("vArquivo", "Você não tem permissão para visualizar arquivos")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var arquivos = await _repositorio.Listar();
            return View("LArquivo", new ArquivoViewModel { Arquivos = arquivos.ToList() });
        }

        [Permissao("aArquivo", "Você não tem permissão para adicionar arquivo")]
        [HttpGet("adicionar")]
        public IActionResult Adicionar()
        {
            return View("AArquivo");
        }

        [Permissao("aArquivo", "Você não tem permissão para adicionar arquivo")]
        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(ArquivoModel arquivo, IFormFile userfile)
        {
            if (!ModelState.IsValid || userfile == null)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios.";
                return View("AArquivo", arquivo);
            }

            var dataCadastro = arquivo.Cadastro != DateTime.MinValue ? arquivo.Cadastro : DateTime.Now;

            var uploadDir = Path.Combine("wwwroot", "arquivos", DateTime.Now.ToString("dd-MM-yyyy"));
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var filePath = Path.Combine(uploadDir, userfile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await userfile.CopyToAsync(stream);
            }

            arquivo.File = userfile.FileName;
            arquivo.Path = filePath;
            arquivo.Url = $"/arquivos/{DateTime.Now:dd-MM-yyyy}/{userfile.FileName}";
            arquivo.Tamanho = (userfile.Length / 1024.0).ToString("F2") + " KB";
            arquivo.Tipo = Path.GetExtension(userfile.FileName);
            arquivo.Cadastro = dataCadastro;

            await _repositorio.Adicionar(arquivo);

            TempData["Success"] = "Arquivo adicionado com sucesso!";
            return RedirectToAction("Index");
        }

        [Permissao("vArquivo", "Você não tem permissão para baixar arquivos")]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var arquivo = await _repositorio.ObterPorId(id);
            if (arquivo == null || !System.IO.File.Exists(arquivo.Path))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(arquivo.Path);
            return File(fileBytes, "application/octet-stream", arquivo.File);
        }

        [Permissao("dArquivo", "Você não tem permissão para excluir arquivos")]
        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var arquivo = await _repositorio.ObterPorId(id);
            if (arquivo == null)
            {
                TempData["Error"] = "Arquivo não encontrado.";
                return RedirectToAction("Index");
            }

            if (System.IO.File.Exists(arquivo.Path))
                System.IO.File.Delete(arquivo.Path);

            await _repositorio.Deletar(arquivo);
            TempData["Success"] = "Arquivo excluído com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
