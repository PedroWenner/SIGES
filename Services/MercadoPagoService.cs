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

                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "A requisição de pagamento está nula!");
                }

                // Criar instâncias para evitar NullReferenceException
                var payer = new PaymentPayerRequest
                {
                    Email = request.Email ?? throw new Exception("Email do cliente não pode ser nulo."),
                    FirstName = request.FirstName ?? "Desconhecido",
                    LastName = request.LastName ?? "Desconhecido",
                    Identification = new IdentificationRequest
                    {
                        Type = request.Identification?.Length == 11 ? "CPF" : "CNPJ",
                        Number = request.Identification ?? throw new Exception("Documento do cliente não pode ser nulo.")
                    },
                    Address = new PaymentPayerAddressRequest
                    {
                        ZipCode = request.ZipCode ?? throw new Exception("CEP não pode ser nulo."),
                        StreetName = request.StreetName ?? throw new Exception("Rua não pode ser nula."),
                        StreetNumber = request.StreetNumber ?? throw new Exception("Número da rua não pode ser nulo."),
                        Neighborhood = request.Neighborhood ?? "Não informado",
                        City = request.City ?? throw new Exception("Cidade não pode ser nula."),
                        FederalUnit = request.FederalUnit ?? throw new Exception("Estado não pode ser nulo.")
                    }
                };

                var paymentRequest = new PaymentCreateRequest
                {
                    TransactionAmount = request.TransactionAmount,
                    Description = request.Description,
                    PaymentMethodId = request.PaymentMethodId,
                    NotificationUrl = "http://alcatech.com.br/",
                    DateOfExpiration = dateOfExpiration,
                    Payer = payer
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
                    Barcode = payment.TransactionDetails.Barcode.Content,
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
                    Acao = "success"
                };
            }
            catch (Exception ex)
            {
                return new { Status = "Erro", Message = ex.Message, Acao = "error" };
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
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            var client = new PaymentClient();
            var payment = await client.GetAsync(Convert.ToInt64(cobranca.ChargeId));

            if (payment == null) throw new Exception("Pagamento não encontrado no Mercado Pago!");

            var canceledPayment = await client.CancelAsync(payment.Id ?? 0);

            if (canceledPayment.Status != "cancelled")
            {
                throw new Exception("Erro ao cancelar pagamento no Mercado Pago!");
            }

            // Atualiza o status no banco
            cobranca.Status = "cancelled";
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Atualiza o status da venda
        /// </summary>
        public async Task<bool> AtualizarStatus(int id)
        {
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            var client = new PaymentClient();
            var payment = await client.GetAsync(Convert.ToInt64(cobranca.ChargeId));

            if (payment == null) throw new Exception("Pagamento não encontrado no Mercado Pago!");

            // Atualiza o status no banco de dados
            cobranca.Status = payment.Status;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
