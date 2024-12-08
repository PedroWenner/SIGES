namespace SIGES.Classes
{
    public class UnidadeMedida
    {
        public string Code { get; set; }
        public string Display { get; set; }

        public UnidadeMedida(string code, string display)
        {
            Code = code;
            Display = display;
        }
    }
}