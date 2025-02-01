using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.ViewModel;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("emitente")]
    public class EmitenteController : Controller
    {
        private readonly IEmitente _emitenteRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public EmitenteController(IEmitente emitenteRepositorio, ILogService logServiceRepositorio)
        {
            _emitenteRepositorio = emitenteRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var emitentes = await _emitenteRepositorio.ObterEmitente();
            var viewModel = new EmitenteViewModel
            {
                Emitente = emitentes ?? new EmitenteModel()
            };

            return View("LEmitente", viewModel);
        }

        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("AEmitente");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(EmitenteModel emitente, IFormFile? imagemEmitente)
        {
            if (!ModelState.IsValid)
            {
                return View("AEmitente", emitente);
            }

            if (imagemEmitente != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemEmitente.CopyToAsync(memoryStream);
                        emitente.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("AEmitente", emitente);
                }
            }

            await _emitenteRepositorio.Adicionar(emitente);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, emitente);

            TempData["Success"] = "Emitente cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var emitente = await _emitenteRepositorio.ObterPorId(id);
            if (emitente == null)
            {
                TempData["Error"] = "Emitente não encontrado.";
                return RedirectToAction("Index");
            }

            return View("EEmitente", emitente);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, EmitenteModel emitente, IFormFile? imagemEmitente)
        {
            if (!ModelState.IsValid)
            {
                return View("EEmitente", emitente);
            }

            var emitenteExistente = await _emitenteRepositorio.ObterPorId(id);
            if (emitenteExistente == null)
            {
                TempData["Error"] = "Emitente não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, emitenteExistente, emitente);

            emitenteExistente.Nome = emitente.Nome;
            emitenteExistente.Cnpj = emitente.Cnpj;
            emitenteExistente.Ie = emitente.Ie;
            emitenteExistente.Cep = emitente.Cep;
            emitenteExistente.Rua = emitente.Rua;
            emitenteExistente.Numero = emitente.Numero;
            emitenteExistente.Bairro = emitente.Bairro;
            emitenteExistente.Cidade = emitente.Cidade;
            emitenteExistente.Uf = emitente.Uf;
            emitenteExistente.Telefone = emitente.Telefone;
            emitenteExistente.Email = emitente.Email;
            if (imagemEmitente != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemEmitente.CopyToAsync(memoryStream);
                        emitenteExistente.ImagemBlob = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao processar a imagem: {ex.Message}");
                    return View("EEmitente", emitente);
                }
            }

            await _emitenteRepositorio.Atualizar(emitenteExistente);

            TempData["Success"] = "Emitente atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var emitente = await _emitenteRepositorio.ObterPorId(id);
            if (emitente == null)
            {
                TempData["Error"] = "Emitente não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, emitente);
            await _emitenteRepositorio.Excluir(id);

            TempData["Success"] = "Emitente excluído com sucesso!";
            return RedirectToAction("Index");
        }
    }
}