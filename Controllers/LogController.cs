using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGES.Data;

namespace SIGES.Controllers
{
    public class LogController : Controller
    {
        private readonly AppDbContext _context;

        public LogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("logs/{entidade}/{entidadeId}")]
        public async Task<IActionResult> ObterLogs(string entidade, int entidadeId)
        {
            var logs = await _context.LogsOperacoes
                .Where(log => log.Entidade == entidade && log.EntidadeId == entidadeId)
                .OrderByDescending(log => log.DataAlteracao)
                .ToListAsync();

            if (!logs.Any())
            {
                return Json(new
                {
                    sucesso = false,
                    mensagem = "Nenhum log encontrado para esta entidade."
                });
            }

            return Json(new
            {
                sucesso = true,
                dados = logs
            });
        }
    }
}
