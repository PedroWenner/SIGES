using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;
using Microsoft.EntityFrameworkCore;

namespace SIGES.Repositorios
{
    public class ConfiguracaoRepositorio : IConfiguracao
    {
        private readonly AppDbContext _context;

        public ConfiguracaoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConfiguracaoModel>> ObterTodasConfiguracoesAsync()
        {
            return await _context.Configuracoes.ToListAsync();
        }

        public async Task<string> ObterValorConfiguracaoAsync(string chave)
        {
            var config = await _context.Configuracoes.FirstOrDefaultAsync(c => c.Config == chave);
            return config?.Valor ?? string.Empty;
        }

        public async Task AtualizarConfiguracaoAsync(string chave, string valor)
        {
            var config = await _context.Configuracoes.FirstOrDefaultAsync(c => c.Config == chave);
            if (config != null)
            {
                config.Valor = valor;
                _context.Configuracoes.Update(config);
            }
            else
            {
                _context.Configuracoes.Add(new ConfiguracaoModel { Config = chave, Valor = valor });
            }
            await _context.SaveChangesAsync();
        }
    }
}