using Microsoft.AspNetCore.Mvc;
using SIGES.Models.Requests;
using SIGES.Services;

namespace SIGES.Controllers
{
    [Route("api/cobrancas")]
    [ApiController]
    public class CobrancaController : ControllerBase
    {
        private readonly CobrancaService _cobrancaService;
        private readonly ILogger<CobrancaController> _logger;

        public CobrancaController(CobrancaService cobrancaService, ILogger<CobrancaController> logger)
        {
            _cobrancaService = cobrancaService;
            _logger = logger;
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> CriarCobranca([FromBody] CobrancaRequest request)
        {
            try
            {
                var cobranca = await _cobrancaService.CriarCobranca(request);
                return Ok(cobranca);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar cobrança: {ex.Message}");
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("visualizar/{id}")]
        public async Task<IActionResult> ObterCobranca(int id)
        {
            var cobranca = await _cobrancaService.ObterCobranca(id);
            if (cobranca == null)
            {
                return NotFound(new { message = "Cobrança não encontrada!" });
            }
            return Ok(cobranca);
        }

        [HttpPost("atualizar")]
        public async Task<IActionResult> AtualizarCobranca([FromBody] AtualizarCobrancaRequest request)
        {
            try
            {
                await _cobrancaService.AtualizarCobranca(request.Id);
                return Ok(new { message = "Cobrança atualizada com sucesso!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("excluir/{id}")]
        public async Task<IActionResult> ExcluirCobranca(int id)
        {
            try
            {
                await _cobrancaService.ExcluirCobranca(id);
                return Ok(new { message = "Cobrança excluída com sucesso!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("confirmar-pagamento")]
        public async Task<IActionResult> ConfirmarPagamento([FromBody] ConfirmarPagamentoRequest request)
        {
            try
            {
                await _cobrancaService.ConfirmarPagamento(request.Id);
                return Ok(new { message = "Pagamento confirmado com sucesso!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("cancelar")]
        public async Task<IActionResult> CancelarCobranca([FromBody] CancelarCobrancaRequest request)
        {
            try
            {
                await _cobrancaService.CancelarCobranca(request.Id);
                return Ok(new { message = "Cobrança cancelada com sucesso!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}