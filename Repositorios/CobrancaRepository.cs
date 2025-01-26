using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;
using SIGES.Models.Requests;

namespace SIGES.Repositorios
{
    public class CobrancaRepository : ICobranca
    {
        private readonly AppDbContext _context;

        public CobrancaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GerarCobranca(CobrancaRequest request)
        {
            var novaCobranca = new Cobranca
            {
                ExpireAt = DateTime.UtcNow.AddDays(3), // Exemplo de vencimento padrão
                Message = "Nova cobrança gerada",
                PaymentMethod = request.FormaPagamento,
                PaymentGateway = request.GatewayDePagamento,
                Status = "Aguardando pagamento",
                Total = request.Valor.ToString("F2"), // Formato de moeda
                Barcode = "00000000000000000000", // Simulação de código de barras
                Link = "http://exemplo.com/pagamento", // Simulação de link de pagamento
                Payment = request.FormaPagamento,
                CreatedAt = DateTime.UtcNow
            };

            _context.Cobrancas.Add(novaCobranca);
            await _context.SaveChangesAsync();

            return novaCobranca;
        }

        public async Task ExcluirCobranca(int id)
        {
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            _context.Cobrancas.Remove(cobranca);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarCobranca(int id)
        {
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            cobranca.Status = "Atualizado";
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmarPagamento(int id)
        {
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            cobranca.Status = "Pago";
            await _context.SaveChangesAsync();
        }

        public async Task CancelarCobranca(int id)
        {
            var cobranca = await _context.Cobrancas.FindAsync(id);
            if (cobranca == null) throw new Exception("Cobrança não encontrada!");

            cobranca.Status = "Cancelado";
            await _context.SaveChangesAsync();
        }

        public async Task<object> ObterCobrancaPorId(int id)
        {
            return await _context.Cobrancas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}