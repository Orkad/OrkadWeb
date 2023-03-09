using OrkadWeb.Application.Common.Interfaces;
using System.Net.Mail;

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
            using var client = new SmtpClient(smtpConfig.Host, smtpConfig.Port);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = smtpConfig.Credentials;
            client.Send("noreply@orkad.fr", to, subject, html);
        }
    }
}
