using Microsoft.AspNetCore.Mvc;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Models.Requests;
using SIGES.Services;
using SIGES.ViewModel;
using System.Text.RegularExpressions;

namespace SIGES.Controllers
{
    [Route("api/cobrancas")]
    [ApiController]
    public class CobrancaController : ControllerBase
    {
        private readonly ICobranca _cobrancaRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IVenda _vendaRepository;
        private readonly IOs _osRepository;

        public CobrancaController(ICobranca cobrancaRepository, IPaymentGateway paymentGateway, IVenda vendaRepository, IOs osRepository)
        {
            _cobrancaRepository = cobrancaRepository;
            _paymentGateway = paymentGateway;
            _vendaRepository = vendaRepository;
            _osRepository = osRepository;
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

                var paymentRequest = new PaymentRequest();
                if (request.Tipo == "venda")
                {
                    var venda = await _vendaRepository.ObterPorId(request.Id);

                    string address = "zip_code = " + Regex.Replace(venda.Cliente.Cep, "[^0-9]", "");
                    address += "street_name = " + venda.Cliente.Rua;
                    address += "street_number = " + venda.Cliente.NumeroRua;
                    address += "neighborhood = " + venda.Cliente.Bairro;
                    address += "city = " + venda.Cliente.Cidade;
                    address += "federal_unit = " + venda.Cliente.Estado;

                    string documento = Regex.Replace(venda.Cliente.Documento, "[^0-9]", "");
                    documento = (documento.Length == 11 ? "CPF" : "CNPJ") + " = " + documento;

                    string name = venda.Cliente.Nome;
                    string[] words = name.Split(' ');

                    paymentRequest = new PaymentRequest
                    {
                        Tipo = request.Tipo,
                        TransactionAmount = venda.ValorTotal,
                        Description = $"Venda #{request.Id}",
                        PaymentMethodId = "bolbradesco",
                        Identification = documento,
                        ZipCode = Regex.Replace(venda.Cliente.Cep, "[^0-9]", ""),
                        StreetName = venda.Cliente.Rua,
                        StreetNumber = venda.Cliente.NumeroRua,
                        Neighborhood = venda.Cliente.Bairro,
                        City = venda.Cliente.Cidade,
                        FederalUnit = venda.Cliente.Estado,
                        Email = venda.Cliente.Email ?? "",
                        ClienteId = venda.Cliente.Id,
                        FirstName = words[0],
                        LastName = words[words.Length - 1],
                    };
                }
                else
                {
                    var os = await _osRepository.ObterPorId(request.Id);
                    
                    string documento = Regex.Replace(os.Cliente.Documento, "[^0-9]", "");
                    string name = os.Cliente.Nome;
                    string[] words = name.Split(' ');

                    paymentRequest = new PaymentRequest
                    {
                        Tipo = request.Tipo,
                        //TransactionAmount = os.,
                        Description = $"Ordem de Serviço #{request.Id}",
                        PaymentMethodId = "bolbradesco",
                        Identification = documento,
                        ZipCode = Regex.Replace(os.Cliente.Cep, "[^0-9]", ""),
                        StreetName = os.Cliente.Rua,
                        StreetNumber = os.Cliente.NumeroRua,
                        Neighborhood = os.Cliente.Bairro,
                        City = os.Cliente.Cidade,
                        FederalUnit = os.Cliente.Estado,
                        Email = os.Cliente.Email ?? "",
                        ClienteId = os.Cliente.Id,
                        FirstName = words[0],
                        LastName = words[words.Length - 1],
                    };
                }

                var cobranca = await _paymentGateway.CriarCobranca(paymentRequest);
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