using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SIGES.Attributes;
using SIGES.Data;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Repositorios
{
    public class LogServiceRepositorio : ILogService
    {
        private readonly AppDbContext _context;

        public LogServiceRepositorio(AppDbContext context)
        {
            _context = context;
        }

            public async Task RegistrarAlteracoes<T>(string usuario, T original, T atualizado) where T : class
            {
                var tipo = typeof(T).Name;
                var id = GetIdValue(original);
                var alteracoes = new List<object>();

                foreach (var prop in typeof(T).GetProperties())
                {
                    // Ignorar propriedades do tipo byte[] (varbinary)
                    if (prop.PropertyType == typeof(byte[]) || Attribute.IsDefined(prop, typeof(IgnoreLogAttribute)))
                    {
                        continue; // Pula esta propriedade
                    }

                    var valorAntigo = prop.GetValue(original)?.ToString();
                    var valorNovo = prop.GetValue(atualizado)?.ToString();

                    if (valorAntigo != valorNovo)
                    {
                        alteracoes.Add(new
                        {
                            Coluna = prop.Name,
                            ValorAnterior = valorAntigo,
                            ValorNovo = valorNovo
                        });
                    }
                }

                if (alteracoes.Any())
                {
                    var log = new LogAlteracaoModel
                    {
                        Usuario = usuario,
                        Entidade = tipo,
                        EntidadeId = id,
                        TipoOperacao = "Alteração",
                        Alteracoes = JsonConvert.SerializeObject(alteracoes)
                    };

                    _context.Add(log);
                    await _context.SaveChangesAsync();
                }
            }

        public async Task RegistrarAdicao<T>(string usuario, T entidade) where T : class
        {
            var tipo = typeof(T).Name;
            var id = GetIdValue(entidade);

            var propriedadesAdicionadas = new List<object>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // Ignorar propriedades do tipo byte[] ou marcadas com IgnoreLog
                if (prop.PropertyType == typeof(byte[]) || Attribute.IsDefined(prop, typeof(IgnoreLogAttribute)))
                {
                    continue;
                }

                var valorAtual = prop.GetValue(entidade)?.ToString();
                propriedadesAdicionadas.Add(new
                {
                    Coluna = prop.Name,
                    ValorNovo = valorAtual
                });
            }

            var log = new LogAlteracaoModel
            {
                Usuario = usuario,
                Entidade = tipo,
                EntidadeId = id,
                TipoOperacao = "Adição",
                Alteracoes = JsonConvert.SerializeObject(propriedadesAdicionadas) // Apenas valores adicionados
            };

            _context.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task RegistrarExclusao<T>(string usuario, T entidade) where T : class
        {
            var tipo = typeof(T).Name;
            var id = GetIdValue(entidade);

            var entidadeFiltrada = FiltrarPropriedades(entidade);

            var log = new LogAlteracaoModel
            {
                Usuario = usuario,
                Entidade = tipo,
                EntidadeId = id,
                TipoOperacao = "Exclusão",
                Alteracoes = JsonConvert.SerializeObject(entidadeFiltrada)
            };

            _context.Add(log);
            await _context.SaveChangesAsync();
        }

        private int GetIdValue<T>(T entidade)
        {
            var prop = typeof(T).GetProperty("Id");
            if (prop == null) throw new Exception("A entidade não possui a propriedade Id.");
            return (int)prop.GetValue(entidade);
        }

        private object FiltrarPropriedades<T>(T entidade) where T : class
        {
            var propriedadesFiltradas = new Dictionary<string, object>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // Ignorar propriedades do tipo byte[] ou marcadas com IgnoreLogAttribute
                if (prop.PropertyType == typeof(byte[]) || Attribute.IsDefined(prop, typeof(IgnoreLogAttribute)))
                {
                    continue;
                }

                propriedadesFiltradas[prop.Name] = prop.GetValue(entidade);
            }

            return propriedadesFiltradas;
        }

        public async Task<List<LogAlteracaoModel>> ObterLogsPorEntidade(string entidade, int entidadeId)
        {
            return await _context.LogsOperacoes
                .Where(l => l.Entidade == entidade && l.EntidadeId == entidadeId)
                .OrderByDescending(l => l.DataAlteracao)
                .ToListAsync();
        }
    }
}