using QRCoder;
using System.Globalization;
using System.Text;

namespace SIGES.Services
{
    public class PixService
    {
        public string GerarQrCode(string chavePix, decimal? valor, string descricao, string nomeEmitente, string cidadeEmitente)
        {
            if (valor <= 0 || string.IsNullOrWhiteSpace(chavePix))
                return string.Empty;

            string valorFormatado = valor.Value.ToString("0.00", CultureInfo.InvariantCulture);
            cidadeEmitente = RemoverAcentos(cidadeEmitente.ToUpper()).Substring(0, Math.Min(cidadeEmitente.Length, 15));
            nomeEmitente = RemoverAcentos(nomeEmitente.ToUpper()).Substring(0, Math.Min(nomeEmitente.Length, 25));

            string payload = $"000201"
            + MontarCampo("26", $"0014BR.GOV.BCB.PIX0114{chavePix}") // Chave Pix correta
            + "52040000" // Código Merchant Category
            + "5303986" // Moeda (BRL)
            + MontarCampo("54", valorFormatado) // Valor da transação
            + "5802BR" // País
            + MontarCampo("59", nomeEmitente) // Nome do recebedor
            + MontarCampo("60", cidadeEmitente) // Cidade do recebedor
            + "62070503***"; // TXID Padrão


            string crc16 = CalcularCRC16(payload + "6304");
            payload += "6304" + crc16;

            Console.WriteLine(payload);

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                return Convert.ToBase64String(qrCode.GetGraphic(20));
            }
        }

        private string MontarCampo(string id, string valor)
        {
            int tamanho = valor.Length.ToString().Length == 1 ? 1 : 2;
            return $"{id}{valor.Length.ToString().PadLeft(tamanho, '0')}{valor}";
        }

        public string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            return new string(texto.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        private string CalcularCRC16(string payload)
        {
            const ushort polynomial = 0x1021;
            ushort crc = 0xFFFF;

            byte[] bytes = Encoding.ASCII.GetBytes(payload);
            foreach (byte b in bytes)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ polynomial);
                    else
                        crc <<= 1;
                }
            }

            return crc.ToString("X4");
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
