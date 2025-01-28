using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models.Requests;
using SIGES.Models;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using SIGES.ViewModel;

namespace SIGES.Services
{
    public class MercadoPagoService : IPaymentGateway
    {
        private readonly PaymentGatewaysService _paymentGatewaysService;
        private readonly AppDbContext _context;

        public MercadoPagoService(PaymentGatewaysService paymentGatewaysService, AppDbContext context)
        {
            _paymentGatewaysService = paymentGatewaysService;
            _context = context;

            // Configura credenciais do MercadoPago
            var config = _paymentGatewaysService.GetGateways()["MercadoPago"];
            MercadoPagoConfig.AccessToken = config.Credentials["access_token"];
            //MercadoPagoConfig.PlatformId = config.Credentials["platform_id"];
        }

        /// <summary>
        /// Cria uma cobrança no MercadoPago e salva no banco de dados.
        /// </summary>
        public async Task<object> CriarCobranca(PaymentRequest request)
        {
            try
            {
                int boletoVencimento = 30;
                string boletoExpiration = _paymentGatewaysService.GetGateways()["MercadoPago"].BoletoExpiration;
                if (int.TryParse(boletoExpiration, out int dias))
                {
                    boletoVencimento = dias;
                }

                DateTime dateOfExpiration = DateTime.Now.AddDays(boletoVencimento);

                var paymentRequest = new PaymentCreateRequest
                {
                    TransactionAmount = request.TransactionAmount,
                    Description = request.Description,
                    PaymentMethodId = request.PaymentMethodId,
                    NotificationUrl = "http://alcatech.com.br/",
                    DateOfExpiration = dateOfExpiration,
                    Payer = new PaymentPayerRequest
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Identification =
                        {
                            Type = request.Identification.Length == 11 ? "CPF" : "CNPJ",
                            Number = request.Identification
                        },
                        Address =
                        {
                            ZipCode = request.ZipCode,
                            StreetName = request.StreetName,
                            StreetNumber = request.StreetNumber,
                            Neighborhood = request.Neighborhood,
                            City = request.City,
                            FederalUnit = request.FederalUnit
                        }
                    }
                };

                var client = new PaymentClient();
                var payment = await client.CreateAsync(paymentRequest);

                if (payment.Status != "approved" && payment.Status != "pending")
                {
                    return new { Status = payment.Status, Message = "Pagamento não aprovado." };
                }

                // Criar a cobrança no banco
                var novaCobranca = new Cobranca
                {
                    Link = payment.TransactionDetails.ExternalResourceUrl ?? "",
                    Pdf = payment.TransactionDetails.ExternalResourceUrl ?? "",
                    ExpireAt = payment.DateOfExpiration ?? DateTime.Now,
                    ChargeId = payment.Id.ToString(),
                    Status = payment.Status,
                    Total = Convert.ToDecimal(request.TransactionAmount).ToString("C"),
                    ClientesId = request.ClienteId,
                    PaymentMethod = "boleto",
                    PaymentGateway = "MercadoPago"
                };

                if (request.Tipo == "venda")
                {
                    novaCobranca.VendasId = request.Id;
                }
                else
                {
                    novaCobranca.OsId = request.Id;
                }

                _context.Cobrancas.Add(novaCobranca);
                await _context.SaveChangesAsync();

                return new
                {
                    Status = "Sucesso",
                    Message = "Cobrança gerada com sucesso!",
                    Cobranca = novaCobranca
                };
            }
            catch (Exception ex)
            {
                return new { Status = "Erro", Message = ex.Message };
            }
        }

        /// <summary>
        /// Confirma um pagamento no MercadoPago.
        /// </summary>
        public async Task<bool> ConfirmarPagamento(int id)
        {
            try
            {
                var cobranca = await _context.Cobrancas.FindAsync(id);
                if (cobranca == null) throw new Exception("Cobrança não encontrada!");

                var client = new PaymentClient();
                var payment = await client.GetAsync(Convert.ToInt64(cobranca.ChargeId));

                if (payment.Status == "approved")
                {
                    cobranca.Status = "Pago";
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cancela uma cobrança no MercadoPago e atualiza no banco de dados.
        /// </summary>
        public async Task<bool> CancelarCobranca(int id)
        {
            try
            {
                var cobranca = await _context.Cobrancas.FindAsync(id);
                if (cobranca == null) throw new Exception("Cobrança não encontrada!");

                var client = new PaymentClient();
                var payment = await client.GetAsync(Convert.ToInt64(cobranca.ChargeId));

                if (payment.Status == "approved")
                {
                    return false; // Se já foi pago, não pode ser cancelado
                }

                //await client.UpdateAsync(payment.Id, new PaymentUpdateRequest { Status = "cancelled" });

                //cobranca.Status = "Cancelado";
                //await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
