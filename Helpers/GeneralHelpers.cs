using HtmlAgilityPack;
using System.Net;
using Microsoft.Security.Application;
using System.Text;
using SIGES.Services;

namespace SIGES.Helpers
{
    public static class GeneralHelpers
    {
        /// <summary>
        /// Obtém o status de uma cobrança baseado no gateway de pagamento e no status retornado.
        /// </summary>
        public static string GetCobrancaTransactionStatus(PaymentGatewaysService paymentGatewaysService, string paymentGateway, string status)
        {
            var gateways = paymentGatewaysService.GetGateways();

            // Verifica se o gateway de pagamento existe nas configurações
            if (gateways.ContainsKey(paymentGateway))
            {
                var gateway = gateways[paymentGateway];

                // Procura o status na lista de TransactionStatus do gateway
                var transactionStatus = gateway.TransactionStatus.Find(ts => ts.Name == status);

                return transactionStatus != null ? transactionStatus.Value : "Status desconhecido";
            }

            return "Gateway não encontrado";
        }
    }
}
