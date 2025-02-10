using Microsoft.AspNetCore.Mvc;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;

namespace SIGES.Controllers.Api
{
    [Route("api/mensagem")]
    [ApiController]
    public class ApiMensagemController : ControllerBase
    {
        private readonly IOs _osRepositorio;
        private readonly IConfiguracao _configuracaoRepositorio;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;

        public ApiMensagemController(IOs osRepositorio, IConfiguracao configuracaoRepositorio, IRepository<EmitenteModel> repositorioEmitente)
        {
            _osRepositorio = osRepositorio;
            _configuracaoRepositorio = configuracaoRepositorio;
            _repositorioEmitente = repositorioEmitente;
        }

        // Chama a API para enviar o zap e retorna status
        [HttpPost("enviar-whatsapp/{id}")]
        public async Task<object> EnviarWhatsapp(int id, [FromBody] MensagemRequest mensagem)
        {
            try
            {
                if (id == 0)
                {
                    return new { status = "error", message = $"Problemas no envio do {id}", acao = "error" };
                }

                var model = await _osRepositorio.ObterPorId(id);
                if (model == null)
                {
                    return new { status = "error", message = "OS/Venda não encontrado.", acao = "error" };
                }

                var emitente = await _repositorioEmitente.GetLastRegister();
                var textoBase = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("notifica_whats");

                var zapnumber = new string(model.Clientes?.Telefone?.Where(char.IsDigit).ToArray());

                List<string> troca = new List<string>
                {
                    model.Clientes?.Nome ?? "",
                    model.Id.ToString() ?? "",
                    model.Status ?? "",
                    "R$ " + (model.Desconto > 0 && model.ValorDesconto > 0
                        ? model.ValorDesconto.ToString("N2")
                        : (model.Produtos?.Sum(x => x.Preco ?? 0) + model.Servicos?.Sum(x => x.Preco ?? 0)).ToString()),
                    model.DescricaoProduto ?? "",
                    emitente?.Nome ?? "",
                    emitente?.Telefone ?? "",
                    model.Observacoes ?? "",
                    model.Defeito ?? "",
                    model.LaudoTecnico ?? "",
                    model.DataFinal?.ToString("dd/MM/yyyy") ?? "",
                    model.DataInicial?.ToString("dd/MM/yyyy") ?? "",
                    $"{model.Garantia} dias"
                };

                string textoDeNotificacao = string.Empty;

                if (!string.IsNullOrEmpty(mensagem?.Mensagem))
                    textoDeNotificacao = mensagem.Mensagem;
                else
                    textoDeNotificacao = HtmlHelpers.CriarTextoWhats(textoBase, troca);

                if (string.IsNullOrEmpty(textoDeNotificacao))
                    return new { status = "error", message = "A mensagem a ser enviada não está configurada.", acao = "error" };

                if (!string.IsNullOrEmpty(zapnumber))
                {
                    string linkWhatsApp = $"https://wa.me/55{zapnumber}?text={textoDeNotificacao}";
                    return new { status = "success", link = linkWhatsApp, acao = "success" };
                }

                return new { status = "error", message = "Não foi possível enviar a mensagem. Telefone não informado.", acao = "error" };

            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message, acao = "error" };
            }
        }
    }

    public class MensagemRequest
    {
        public string? Mensagem { get; set; }
    }
}
