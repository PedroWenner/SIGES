using SIGES.Interfaces;
using SIGES.Models.Requests;

namespace SIGES.Services
{
    public class CobrancaService
    {
        private readonly ICobranca _cobrancaRepository;

        public CobrancaService(ICobranca cobrancaRepository)
        {
            _cobrancaRepository = cobrancaRepository;
        }

        public async Task<object> CriarCobranca(CobrancaRequest request)
        {
            return await _cobrancaRepository.GerarCobranca(request);
        }

        public async Task ExcluirCobranca(int id)
        {
            await _cobrancaRepository.ExcluirCobranca(id);
        }

        public async Task AtualizarCobranca(int id)
        {
            await _cobrancaRepository.AtualizarCobranca(id);
        }

        public async Task ConfirmarPagamento(int id)
        {
            await _cobrancaRepository.ConfirmarPagamento(id);
        }

        public async Task CancelarCobranca(int id)
        {
            await _cobrancaRepository.CancelarCobranca(id);
        }

        public async Task<object> ObterCobranca(int id)
        {
            return await _cobrancaRepository.ObterCobrancaPorId(id);
        }
    }
}