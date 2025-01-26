namespace SIGES.Services
{
    public class PaymentGatewaysService
    {
        private readonly AppSettingsService _appSettingsService;
        public PaymentGatewaysService(AppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        /// <summary>
        /// Obtém todas as configurações dos gateways de pagamento do appsettings.json.
        /// </summary>
        /// <returns>Um dicionário com os gateways configurados.</returns>
        public Dictionary<string, PaymentGatewayConfig> GetGateways()
        {
            var configuracoesViewModel = new Dictionary<string, string>();
            var configuracoesAppSettings = new List<string>
            {
                "IMPRIMIR_ANEXOS",
                "PAYMENT_GATEWAYS_EFI_PRODUCTION",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_ID",
                "PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_EFI_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_PUBLIC_KEY",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_ACCESS_TOKEN",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_ID",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_SECRET",
                "PAYMENT_GATEWAYS_MERCADO_PAGO_BOLETO_EXPIRATION",
                "PAYMENT_GATEWAYS_ASAAS_PRODUCTION",
                "PAYMENT_GATEWAYS_ASAAS_NOTIFY",
                "PAYMENT_GATEWAYS_ASAAS_CREDENTIAIS_API_KEY",
                "PAYMENT_GATEWAYS_ASAAS_BOLETO_EXPIRATION",
                "API_ENABLED",
                "API_TOKEN_EXPIRE_TIME",
                "API_JWT_KEY",
                "EMAIL_PROTOCOL",
                "EMAIL_SMTP_HOST",
                "EMAIL_SMTP_CRYPTO",
                "EMAIL_SMTP_PORT",
                "EMAIL_SMTP_USER",
                "EMAIL_SMTP_PASS"
            };

            // Carregar as configurações do appsettings.json
            foreach (var chave in configuracoesAppSettings)
            {
                configuracoesViewModel[chave] = _appSettingsService.ObterConfiguracao(chave);
            }

            return new Dictionary<string, PaymentGatewayConfig>
            {
                { "GerencianetSdk", new PaymentGatewayConfig
                    {
                        Name = "GerenciaNet (Efí)",
                        LibraryName = "GerencianetSdk",
                        Production = RetornaBoolDeSimNao(configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_EFI_PRODUCTION")),
                        Credentials = new Dictionary<string, string>
                        {
                            { "client_id", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_ID") ?? "" },
                            { "client_secret", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_EFI_CREDENTIAIS_CLIENT_SECRET") ?? "" }
                        },
                        TimeOut = configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_EFI_TIMEOUT") ?? "30",
                        BoletoExpiration = configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_EFI_BOLETO_EXPIRATION") ?? "P3D",
                        PaymentMethods = new List<PaymentMethod>
                        {
                            new PaymentMethod { Name = "Boleto", Value = "boleto" },
                            new PaymentMethod { Name = "Link", Value = "link" }
                        },
                        TransactionStatus = new List<TransactionStatus>
                        {
                            new TransactionStatus { Name = "new", Value = "Cobrança / Assinatura gerada" },
                            new TransactionStatus { Name = "waiting", Value = "Aguardando a confirmação do pagamento" },
                            new TransactionStatus { Name = "paid", Value = "Pagamento confirmado" },
                            new TransactionStatus { Name = "idenfied", Value = "Pagamento identificado" },
                            new TransactionStatus { Name = "unpaid", Value = "Não foi possível confirmar o pagamento da cobrança" },
                            new TransactionStatus { Name = "refunded", Value = "Pagamento devolvido pelo lojista ou pelo intermediador Gerencianet" },
                            new TransactionStatus { Name = "contested", Value = "Pagamento em processo de contestação" },
                            new TransactionStatus { Name = "canceled", Value = "Cobrança/Assinatura cancelada pelo vendedor ou pelo pagador" },
                            new TransactionStatus { Name = "settled", Value = "Cobrança/Pagamento foi confirmada manualmente" },
                            new TransactionStatus { Name = "link", Value = "Link de pagamento" },
                            new TransactionStatus { Name = "expired", Value = "Link/Assinatura de pagamento expirado" },
                            new TransactionStatus { Name = "active", Value = "Assinatura ativa Todas as cobranças estão sendo geradas" },
                            new TransactionStatus { Name = "finished", Value = "Carnê está finalizado" },
                            new TransactionStatus { Name = "up_to_date", Value = "Carnê encontra-se em dia" }
                        }
                    }
                },
                { "MercadoPago", new PaymentGatewayConfig
                    {
                        Name = "MercadoPago",
                        LibraryName = "MercadoPago",
                        Credentials = new Dictionary<string, string>
                        {
                            { "access_token", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_ACCESS_TOKEN") ?? "" },
                            { "public_key", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_PUBLIC_KEY") ?? "" },
                            { "client_secret", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_SECRET") ?? "" },
                            { "client_id", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CLIENT_ID") ?? "" },
                            { "integrator_id", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_INTEGRATOR_ID") ?? "" },
                            { "platform_id", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_PLATFORM_ID") ?? "" },
                            { "corporation_id", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_CREDENTIALS_CORPORATION_ID") ?? "" },
                            
                        },
                        BoletoExpiration = configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_MERCADO_PAGO_BOLETO_EXPIRATION") ?? "P3D",
                        PaymentMethods = new List<PaymentMethod>
                        {
                            new PaymentMethod { Name = "Boleto", Value = "boleto" }
                        },
                        TransactionStatus = new List<TransactionStatus>
                        {
                            new TransactionStatus { Name = "pending", Value = "O usuário ainda não concluiu o processo de pagamento" },
                            new TransactionStatus { Name = "approved", Value = "O pagamento foi aprovado e credenciado" },
                            new TransactionStatus { Name = "authorized", Value = "O pagamento foi autorizado, mas ainda não foi capturado" },
                            new TransactionStatus { Name = "in_process", Value = "O pagamento está sendo revisado" },
                            new TransactionStatus { Name = "in_mediation", Value = "Os usuários iniciaram uma disputa" },
                            new TransactionStatus { Name = "rejected", Value = "O pagamento foi rejeitado, o usuário pode tentar o pagamento novamente" },
                            new TransactionStatus { Name = "cancelled", Value = "O pagamento foi cancelado por uma das partes ou porque o prazo para pagamento expirou" },
                            new TransactionStatus { Name = "refunded", Value = "O pagamento foi reembolsado ao usuário" },
                            new TransactionStatus { Name = "charged_back", Value = "Foi feito um estorno no cartão de crédito do comprador" }
                        }
                    }
                },
                { "Asaas", new PaymentGatewayConfig
                    {
                        Name = "Asaas",
                        LibraryName = "Asaas",
                        Production = RetornaBoolDeSimNao(configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_ASAAS_PRODUCTION")),
                        Credentials = new Dictionary<string, string>
                        {
                            { "api_key", configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_ASAAS_CREDENTIAIS_API_KEY") ?? "" }
                        },
                        BoletoExpiration = configuracoesViewModel.GetValueOrDefault("PAYMENT_GATEWAYS_ASAAS_BOLETO_EXPIRATION") ?? "P3D",
                        PaymentMethods = new List<PaymentMethod>
                        {
                            new PaymentMethod { Name = "Boleto", Value = "boleto" },
                            new PaymentMethod { Name = "Link", Value = "link" }
                        },
                        TransactionStatus = new List<TransactionStatus>
                        {
                            new TransactionStatus { Name = "PENDING", Value = "Aguardando pagamento" },
                            new TransactionStatus { Name = "RECEIVED", Value = "Recebida (saldo já creditado na conta)" },
                            new TransactionStatus { Name = "CONFIRMED", Value = "Pagamento confirmado (saldo ainda não creditado)" },
                            new TransactionStatus { Name = "OVERDUE", Value = "Vencida" },
                            new TransactionStatus { Name = "REFUNDED", Value = "Estornada" },
                            new TransactionStatus { Name = "RECEIVED_IN_CASH", Value = "Recebida em dinheiro (não gera saldo na conta)" },
                            new TransactionStatus { Name = "REFUND_REQUESTED", Value = "Estorno Solicitado" },
                            new TransactionStatus { Name = "CHARGEBACK_REQUESTED", Value = "Recebido chargeback" },
                            new TransactionStatus { Name = "CHARGEBACK_DISPUTE", Value = "Em disputa de chargeback (caso sejam apresentados documentos para contestação)" },
                            new TransactionStatus { Name = "AWAITING_CHARGEBACK_REVERSAL", Value = "Disputa vencida, aguardando repasse da adquirente" },
                            new TransactionStatus { Name = "DUNNING_REQUESTED", Value = "Em processo de recuperação" },
                            new TransactionStatus { Name = "DUNNING_RECEIVED", Value = "Recuperada" },
                            new TransactionStatus { Name = "AWAITING_RISK_ANALYSIS", Value = "Pagamento em análise" },
                        }
                    }
                }
            };
        }

        public bool RetornaBoolDeSimNao(string valor)
        {
            if (valor == "true")
            {
                return true;
            }

            return false;
        }
    }
}

/// <summary>
/// Representa a configuração de um Gateway de Pagamento.
/// </summary>
public class PaymentGatewayConfig
{
    public string Name { get; set; } = string.Empty;
    public string LibraryName { get; set; } = string.Empty;
    public bool Production { get; set; } = false;
    public Dictionary<string, string> Credentials { get; set; } = new Dictionary<string, string>();
    public string TimeOut { get; set; } = "30";
    public string BoletoExpiration { get; set; } = "P3D";
    public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    public List<TransactionStatus> TransactionStatus { get; set; } = new List<TransactionStatus>();
}

/// <summary>
/// Representa um método de pagamento dentro de um Gateway.
/// </summary>
public class PaymentMethod
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Representa um método de pagamento dentro de um Gateway.
/// </summary>
public class TransactionStatus
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}