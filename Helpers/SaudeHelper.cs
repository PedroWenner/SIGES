namespace SIGES.Helpers
{
    public class SaudeHelper
    {
        public static string ObterSaudacao()
        {
            var horaAtual = DateTime.Now.TimeOfDay;

            if (horaAtual >= new TimeSpan(0, 1, 0) && horaAtual < new TimeSpan(12, 0, 0))
                return "Bom dia";
            else if (horaAtual >= new TimeSpan(12, 1, 0) && horaAtual < new TimeSpan(18, 0, 0))
                return "Boa tarde";
            else
                return "Boa noite";
        }
    }
}
