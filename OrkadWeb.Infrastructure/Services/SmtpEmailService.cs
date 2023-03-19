using OrkadWeb.Application.Common.Interfaces;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpConfig smtpConfig;

        public SmtpEmailService(SmtpConfig smtpConfig)
        {
            this.smtpConfig = smtpConfig;
        }

        public void Send(string to, string subject, string html)
        {
            using var client = BuildClient();
            client.Send("noreply@orkad.fr", to, subject, html);
        }

        public async Task SendAsync(string to, string subject, string html, CancellationToken cancellationToken = default)
        {
            using var client = BuildClient();
            await client.SendMailAsync("noreply@orkad.fr", to, subject, html, cancellationToken);
        }

        private SmtpClient BuildClient()
        {
            var client = new SmtpClient(smtpConfig.Host, smtpConfig.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = smtpConfig.Credentials
            };
            return client;
        }
    }
}
