using Microsoft.EntityFrameworkCore;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Repositorios
{
    public class EmailQueueRepository : IEmailQueueRepository
    {
        private readonly AppDbContext _context;

        public EmailQueueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarEmail(EmailQueue email)
        {
            await _context.EmailQueue.AddAsync(email);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmailQueue>> ObterEmailsPendentes()
        {
            return await _context.EmailQueue.Where(e => e.Status == "pending").ToListAsync();
        }

        public async Task AtualizarStatus(int id, string status)
        {
            var email = await _context.EmailQueue.FindAsync(id);
            if (email != null)
            {
                email.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EmailQueue>> Listar()
        {
            return await _context.EmailQueue.ToListAsync();
        }
    }
}
