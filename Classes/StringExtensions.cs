namespace SIGES.Classes
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
        }
    }
}
