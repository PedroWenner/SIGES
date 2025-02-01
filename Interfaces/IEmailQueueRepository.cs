using SIGES.Models;

namespace SIGES.Interfaces
{
    public interface IEmailQueueRepository
    {
        Task AdicionarEmail(EmailQueue email);
        Task<List<EmailQueue>> ObterEmailsPendentes();
        Task AtualizarStatus(int id, string status);
        Task<List<EmailQueue>> Listar();
    }
}
