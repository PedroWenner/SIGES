using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Classes;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.ViewModel;
using System.Text.Json;

namespace SIGES.Controllers
{
    [Authorize]
    [Route("clientes")]
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly ILogService _logServiceRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio, ILogService logServiceRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _logServiceRepositorio = logServiceRepositorio;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var clientes = _clienteRepositorio.Listar();
            var viewModel = new ClienteViewModel
            {
                Clientes = clientes.ToList(),
            };

            var permissoesUsuario = HttpContext.Items["Permissoes"] as List<PermissaoDetalheModel>;
            ViewData["PermissoesUsuario"] = permissoesUsuario;

            return View("LCliente", viewModel);
        }

        [Permissao("aCliente")]
        [HttpGet("adicionar")]
        public async Task<IActionResult> Adicionar()
        {
            return View("ACliente");
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar(ClienteModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return View("ACliente", cliente);
            }

            await _clienteRepositorio.Adicionar(cliente);
            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAdicao(usuarioAtual, cliente);

            TempData["Success"] = "Cliente cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [Permissao("eCliente")]
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _clienteRepositorio.ObterPorId(id);

            if (cliente == null)
            {
                TempData["Error"] = "Cliente não encontrado.";
                return RedirectToAction("Index");
            }

            return View("ECliente", cliente);
        }

        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Editar(int id, ClienteModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return View("ECliente", cliente);
            }

            var clienteExistente = await _clienteRepositorio.ObterPorId(id);

            if (clienteExistente == null)
            {
                TempData["Error"] = "Cliente não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarAlteracoes(usuarioAtual, clienteExistente, cliente);

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Documento = cliente.Documento;
            clienteExistente.Contato = cliente.Contato;
            clienteExistente.Telefone = cliente.Telefone;
            clienteExistente.Celular = cliente.Celular;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Senha = cliente.Senha;
            clienteExistente.Fornecedor = cliente.Fornecedor;
            clienteExistente.PessoaFisica = cliente.PessoaFisica;
            clienteExistente.Cep = cliente.Cep;
            clienteExistente.Rua = cliente.Rua;
            clienteExistente.NumeroRua = cliente.NumeroRua;
            clienteExistente.Complemento = cliente.Complemento;
            clienteExistente.Bairro = cliente.Bairro;
            clienteExistente.Cidade = cliente.Cidade;
            clienteExistente.Estado = cliente.Estado;

            await _clienteRepositorio.Atualizar(clienteExistente);

            TempData["Success"] = "Cliente atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            var cliente = await _clienteRepositorio.ObterPorId(id);

            if (cliente == null)
            {
                TempData["Error"] = "Cliente não encontrado.";
                return RedirectToAction("Index");
            }

            var usuarioAtual = User.Identity.IsAuthenticated ? User.Identity.Name : "Usuário";
            await _logServiceRepositorio.RegistrarExclusao(usuarioAtual, cliente);
            await _clienteRepositorio.Excluir(id);

            TempData["Success"] = "Cliente excluído com sucesso!";

            return RedirectToAction("Index");
        }
    }
}
