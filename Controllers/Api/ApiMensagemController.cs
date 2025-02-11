using Microsoft.AspNetCore.Mvc;
using SIGES.Helpers;
using SIGES.Interfaces;
using SIGES.Models;
using SIGES.Repositorios;
using SIGES.Services;

namespace SIGES.Controllers.Api
{
    [Route("api/mensagem")]
    [ApiController]
    public class ApiMensagemController : ControllerBase
    {
        private readonly IOs _osRepositorio;
        private readonly IConfiguracao _configuracaoRepositorio;
        private readonly IRepository<EmitenteModel> _repositorioEmitente;
        private readonly IRepository<UsuarioModel> _repositorioUsuario;
        private readonly IRepository<EmailQueue> _repositorioEmail;
        private readonly IViewRenderService _viewRenderService;

        public ApiMensagemController(IOs osRepositorio, IConfiguracao configuracaoRepositorio, IRepository<EmitenteModel> repositorioEmitente,
            IViewRenderService viewRenderService, IRepository<UsuarioModel> repositorioUsuario, IRepository<EmailQueue> repositorioEmail)
        {
            _osRepositorio = osRepositorio;
            _configuracaoRepositorio = configuracaoRepositorio;
            _repositorioEmitente = repositorioEmitente;
            _viewRenderService = viewRenderService;
            _repositorioUsuario = repositorioUsuario;
            _repositorioEmail = repositorioEmail;
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

        [HttpPost("enviar-email/{id}")]
        public async Task<object> EnviarEmail(int id, [FromBody] EmailRequest email)
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
                if (string.IsNullOrEmpty(emitente?.Email))
                {
                    return new { status = "error", message = "E-mail do emitente não cadastrado.", acao = "error" };
                }

                var produtos = _osRepositorio.GetProdutos(id);
                var servicos = _osRepositorio.GetServicos(id);

                // **Renderizando a View HTML**
                var viewmodel = new OsEmailViewModel
                {
                    Os = model,
                    Emitente = emitente,
                    Produtos = produtos.ToList(),
                    Servicos = servicos.ToList(),
                };
                var htmlBody = await _viewRenderService.RenderToStringAsync(email.Nomeview, viewmodel);

                var tecnico = await _repositorioUsuario.ObterPorId(model.UsuariosId);
                var osNotification = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("os_notification");
                var emailAutomatico = await _configuracaoRepositorio.ObterValorConfiguracaoAsync("email_automatico");

                List<string> remetentes = DefinirRemetentes(osNotification, model?.Clientes?.Email ?? "", tecnico?.Email ?? "", emitente.Email);

                if (osNotification != "nenhum" && emailAutomatico == "1" && email.AutomaticoMail == "S")
                {
                    foreach (var remetente in remetentes.Where(r => !string.IsNullOrEmpty(r)))
                    {
                        await _repositorioEmail.Adicionar(new EmailQueue
                        {
                            To = remetente,
                            Message = htmlBody,
                            Status = "pending",
                            Date = DateTime.UtcNow
                        });
                    }

                    return new { status = "success", message = "E-mail enviado a fila com sucesso!", acao = "success" };
                }
                else if (email.AutomaticoMail == "N")
                {
                    foreach (var remetente in remetentes.Where(r => !string.IsNullOrEmpty(r)))
                    {
                        await _repositorioEmail.Adicionar(new EmailQueue
                        {
                            To = remetente,
                            Message = htmlBody,
                            Status = "pending",
                            Date = DateTime.UtcNow
                        });
                    }

                    return new { status = "success", message = "E-mail enviado a fila com sucesso!", acao = "success" };
                }

                return new { status = "error", message = "Erro ao enviar e-mail!", acao = "error" };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message, acao = "error" };
            }
        }

        private List<string> DefinirRemetentes(string configuracao, string emailCliente, string emailTecnico, string emailEmitente)
        {
            var remetentes = new List<string>();

            switch (configuracao)
            {
                case "todos":
                    remetentes.AddRange(new[] { emailCliente, emailTecnico, emailEmitente });
                    break;
                case "cliente":
                    remetentes.Add(emailCliente);
                    break;
                case "tecnico":
                    remetentes.Add(emailTecnico);
                    break;
                case "emitente":
                    remetentes.Add(emailEmitente);
                    break;
                default:
                    remetentes.Add(emailCliente);
                    break;
            }

            return remetentes.Distinct().Where(r => !string.IsNullOrEmpty(r)).ToList();
        }
    }

    public class MensagemRequest
    {
        public string? Mensagem { get; set; }
        public string? Tipo { get; set; }
    }
    public class EmailRequest
    {
        public string? Assunto { get; set; }
        public string? Tipo { get; set; }
        public string? Nomeview { get; set; }
        public string? AutomaticoMail { get; set; }
    }
}
