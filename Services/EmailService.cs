using SIGES.Interfaces;
using SIGES.Models;
using System.Net.Mail;
using System.Net;

namespace SIGES.Services
{
    public class EmailService
    {
        private readonly IEmailQueueRepository _emailQueueRepository;
        private readonly AppSettingsService _appSettingsService;

        public EmailService(IEmailQueueRepository emailQueueRepository, AppSettingsService appSettingsService)
        {
            _emailQueueRepository = emailQueueRepository;
            _appSettingsService = appSettingsService;
        }

        public async Task ProcessarFilaDeEmails()
        {
            var emailsPendentes = await _emailQueueRepository.ObterEmailsPendentes();
            foreach (var email in emailsPendentes)
            {
                try
                {
                    EnviarEmail(email);
                    await _emailQueueRepository.AtualizarStatus(email.Id, "sent");
                }
                catch (Exception)
                {
                    await _emailQueueRepository.AtualizarStatus(email.Id, "failed");
                }
            }
        }

        private void EnviarEmail(EmailQueue email)
        {
            var smtp = _appSettingsService.ObterConfiguracao("EMAIL_SMTP_HOST");
            var user = _appSettingsService.ObterConfiguracao("EMAIL_SMTP_USER");
            var pass = _appSettingsService.ObterConfiguracao("EMAIL_SMTP_PASS");
            var crypto = _appSettingsService.ObterConfiguracao("EMAIL_SMTP_CRYPTO") == "ssl" ? true : false;
            var portaConfigurada = _appSettingsService.ObterConfiguracao("EMAIL_SMTP_PORT");
            if (!int.TryParse(portaConfigurada, out int port) || port == 0)
            {
                port = 587; // Define um valor padrão seguro
            }

            var smtpClient = new SmtpClient(smtp)
            {
                Port = port,
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = crypto
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(user),
                Subject = "Cobrança",
                Body = email.Message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email.To);
            if (!string.IsNullOrEmpty(email.Cc)) mailMessage.CC.Add(email.Cc);
            if (!string.IsNullOrEmpty(email.Bcc)) mailMessage.Bcc.Add(email.Bcc);

            smtpClient.Send(mailMessage);
        }
    }
}
