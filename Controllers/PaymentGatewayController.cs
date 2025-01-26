using Microsoft.AspNetCore.Mvc;
using SIGES.Services;

namespace SIGES.Controllers
{
    [Route("api/payment-gateways")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly PaymentGatewaysService _paymentGatewaysService;

        public PaymentGatewayController(PaymentGatewaysService paymentGatewaysService)
        {
            _paymentGatewaysService = paymentGatewaysService;
        }

        /// <summary>
        /// Retorna todos os gateways de pagamento disponíveis.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllGateways()
        {
            var gateways = _paymentGatewaysService.GetGateways();
            return Ok(gateways);
        }

        /// <summary>
        /// Retorna os métodos de pagamento de um gateway específico.
        /// </summary>
        [HttpGet("{gateway}/methods")]
        public IActionResult GetPaymentMethods(string gateway)
        {
            var gateways = _paymentGatewaysService.GetGateways();

            if (gateways.ContainsKey(gateway))
            {
                return Ok(gateways[gateway].PaymentMethods);
            }

            return NotFound(new { message = "Gateway não encontrado" });
        }
    }
}
