using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGES.Attributes;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.Services;
using SIGES.ViewModel;
    
namespace SIGES.Controllers
{
    [Authorize]
    [Route("cobrancas")]
    public class CobrancaController : Controller
    {
        private readonly ICobranca _cobrancaRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly PaymentGatewaysService _paymentGatewaysService;
        private readonly IEmailQueueRepository _emailQueueRepository;
        private readonly IClienteRepositorio _clienteRepository;
        private readonly IViewRenderService _viewRenderService;

        public CobrancaController(ICobranca cobrancaRepository, IPaymentGateway paymentGateway, PaymentGatewaysService paymentGatewaysService,
            IEmailQueueRepository emailQueueRepository, IClienteRepositorio clienteRepository, IViewRenderService viewRenderService)
        {
            _cobrancaRepository = cobrancaRepository;
            _paymentGateway = paymentGateway;
            _paymentGatewaysService = paymentGatewaysService;
            _emailQueueRepository = emailQueueRepository;
            _clienteRepository = clienteRepository;
            _viewRenderService = viewRenderService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var cobrancas = await _cobrancaRepository.Listar();
            var cobrancasViewModel = new CobrancaViewModel
            {
                Cobrancas = cobrancas.Select(c => new CobrancaItemViewModel
                {
                    Id = c.Id,
                    PaymentGateway = c.PaymentGateway,
                    PaymentMethod = c.PaymentMethod,
                    ExpireAt = c.ExpireAt,
                    OsId = c.OsId,
                    Status = c.Status,
                    StatusDescricao = GeneralHelpers.GetCobrancaTransactionStatus(_paymentGatewaysService, c.PaymentGateway, c.Status),
                    Total = c.Total,
                    VendasId = c.VendasId,
                    Barcode = c.Barcode,
                    Link = c.Link
                }).ToList()
            };

            return View("LCobranca", cobrancasViewModel);
        }

        [HttpPost("excluir")]
        [Permissao("dCobranca", "Você não tem permissão para excluir cobranças!")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var cobranca = await _cobrancaRepository.ObterCobrancaPorId(id);
                if (cobranca == null)
                {
                    TempData["Error"] = "Cobrança não encontrada!";
                    return RedirectToAction("Index");
                }

                // Cancela o pagamento no gateway antes de excluir
                bool cancelado = await _paymentGateway.CancelarCobranca(id);
                if (!cancelado)
                {
                    TempData["Error"] = "Erro ao cancelar pagamento no Mercado Pago!";
                    return RedirectToAction("Index");
                }

                // Exclui a cobrança do banco de dados
                await _cobrancaRepository.ExcluirCobranca(id);
                TempData["Success"] = "Cobrança excluída com sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [Permissao("eCobranca", "Você não tem permissão para atualizar cobrança!")]
        [HttpGet("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id)
        {
            try
            {
                var cobranca = await _cobrancaRepository.ObterCobrancaPorId(id);
                if (cobranca == null)
                {
                    TempData["Error"] = "Cobrança não encontrada!";
                    return RedirectToAction("Index");
                }

                // Verifica o status da cobrança no Mercado Pago e atualiza no banco
                var atualizado = await _paymentGateway.AtualizarStatus(id);

                if (atualizado)
                {
                    TempData["Success"] = "Cobrança atualizada com sucesso!";
                }
                else
                {
                    TempData["Error"] = "Erro ao atualizar cobrança!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Permissao("eCobranca", "Você não tem permissão para cancelar cobrança!")]
        [HttpPost("cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var cobranca = await _cobrancaRepository.ObterCobrancaPorId(id);
                if (cobranca == null)
                {
                    TempData["Error"] = "Cobrança não encontrada!";
                    return RedirectToAction("Index");
                }

                // Verifica o status da cobrança no Mercado Pago e atualiza no banco
                var atualizado = await _paymentGateway.CancelarCobranca(id);
                atualizado = await _paymentGateway.AtualizarStatus(id);

                if (atualizado)
                {
                    TempData["Success"] = "Cobrança cancelada com sucesso!";
                }
                else
                {
                    TempData["Error"] = "Erro ao cancelar cobrança!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Permissao("eCobranca", "Você não tem permissão para confirmar cobrança!")]
        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarPagamento(int id)
        {
            try
            {
                var cobranca = await _cobrancaRepository.ObterCobrancaPorId(id);
                if (cobranca == null)
                {
                    TempData["Error"] = "Cobrança não encontrada!";
                    return RedirectToAction("Index");
                }

                // Verifica o status da cobrança no Mercado Pago e atualiza no banco
                var (atualizado, msg) = await _paymentGateway.ConfirmarPagamento(id);

                if (atualizado)
                {
                    atualizado = await _paymentGateway.AtualizarStatus(id);
                    TempData["Success"] = "Cobrança confirmada com sucesso!";
                }
                else
                {
                    TempData["Error"] = msg;
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Permissao("vCobranca", "Você não tem permissão para enviar e-mail!")]
        [HttpPost("enviar-email/{id}")]
        public async Task<IActionResult> EnviarEmail(int id)
        {
            try
            {
                var cobrancaObj = await _cobrancaRepository.ObterCobrancaPorId(id);
                if (cobrancaObj == null)
                {
                    TempData["Error"] = "Cobrança não encontrada!";
                    return RedirectToAction("Index");
                }

                var cobranca = cobrancaObj as Cobranca;
                if (cobranca == null)
                {
                    TempData["Error"] = "Erro ao processar cobrança!";
                    return RedirectToAction("Index");
                }

                var cliente = await _clienteRepository.ObterPorId(Convert.ToInt32(cobranca.ClientesId));
                if (cliente == null)
                {
                    TempData["Error"] = "Erro ao processar cobrança!";
                    return RedirectToAction("Index");
                }

                // **Renderizando a View HTML**
                var model = new CobrancaEmailViewModel
                {
                    Cobranca = cobranca,
                    Cliente = cliente
                };
                var htmlBody = await _viewRenderService.RenderToStringAsync("Email/CobrancaEmail", model);

                var email = new EmailQueue
                {
                    To = cliente.Email,
                    Message = htmlBody,
                    Status = "pending",
                    Date = DateTime.UtcNow
                };

                await _emailQueueRepository.AdicionarEmail(email);

                return Ok(new { status = "Sucesso!", message = "E-mail adicionado à fila!", acao = "success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Erro!", message = ex.Message, acao = "error", stackTrace = ex.StackTrace });
            }
        }
    }
}