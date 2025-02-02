using QRCoder;

namespace SIGES.Services
{
    public class PixService
    {
        public string GerarQrCode(string chavePix, decimal? valor, string descricao, string nomeEmitente, string cidadeEmitente)
        {
            if (valor <= 0 || string.IsNullOrWhiteSpace(chavePix))
                return string.Empty;

            var payload = $"00020126360014BR.GOV.BCB.PIX0114{chavePix}520400005303986540{valor:0.00}5802BR5909{nomeEmitente}6009{cidadeEmitente}62070503***6304";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new Base64QRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
        }

        public string FormatarChavePix(string chave)
        {
            if (chave.Length == 11) // CPF
                return Convert.ToUInt64(chave).ToString(@"000\.000\.000\-00");
            else if (chave.Length == 14) // CNPJ
                return Convert.ToUInt64(chave).ToString(@"00\.000\.000\/0000\-00");
            else if (chave.Length == 32)
                return chave.Insert(8, "-");
            return chave;
        }
    }
}
