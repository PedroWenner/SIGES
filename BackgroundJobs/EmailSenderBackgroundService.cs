using SIGES.Services;

namespace SIGES.BackgroundJobs
{
    public class EmailSenderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailSenderBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    await emailService.ProcessarFilaDeEmails();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
