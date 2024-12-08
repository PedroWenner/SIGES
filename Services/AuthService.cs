namespace SIGES.Services
{
    public class AuthService
    {
        public bool VerificarSenha(string senhaHashed, string senhaBruta)
        {
            return BCrypt.Net.BCrypt.Verify(senhaBruta, senhaHashed);
        }
    }
}