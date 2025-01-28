using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Models.Requests;
using SIGES.Services;

namespace SIGES.Controllers
{
    [Route("api/cobrancas")]
    [ApiController]
    public class CobrancaController : ControllerBase
    {
        private readonly ICobranca _cobrancaRepository;
        private readonly IPaymentGateway _paymentGateway;

        public CobrancaController(ICobranca cobrancaRepository, IPaymentGateway paymentGateway)
        {
            _cobrancaRepository = cobrancaRepository;
            _paymentGateway = paymentGateway;
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> CriarCobranca([FromBody] CobrancaRequest request)
        {
            try
            {
                var cobrancas = await _cobrancaRepository.ObterTodasCobrancas();
                var cobrancasExistentes = cobrancas.Where(x => (request.Tipo == "venda" ? x.VendasId : x.OsId) == request.Id).ToList();

                if (cobrancasExistentes.Any())
                {
                    return BadRequest(new { message = "Já existe uma cobrança!" });
                }

                var cobranca = await _paymentGateway.CriarCobranca(request);
                return Ok(cobranca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("confirmar-pagamento/{id}")]
        public async Task<IActionResult> ConfirmarPagamento(int id)
        {
            try
            {
                bool confirmado = await _paymentGateway.ConfirmarPagamento(id);
                if (confirmado)
                    return Ok(new { message = "Pagamento confirmado!" });

                return BadRequest(new { message = "Pagamento não confirmado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("cancelar/{id}")]
        public async Task<IActionResult> CancelarCobranca(int id)
        {
            try
            {
                bool cancelado = await _paymentGateway.CancelarCobranca(id);
                if (cancelado)
                    return Ok(new { message = "Cobrança cancelada!" });

                return BadRequest(new { message = "Cobrança não pode ser cancelada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}