using Microsoft.Extensions.Configuration;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Extensions;
using System.Net;
using System.Net.Mail;

namespace OrkadWeb.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        public const string CONFIG_SMTP_HOST = "Smtp:Host";
        public const string CONFIG_SMTP_PORT = "Smtp:Port";
        public const string CONFIG_SMTP_USERNAME = "Smtp:Username";
        public const string CONFIG_SMTP_PASSWORD = "Smtp:Password";
        private readonly IConfiguration configuration;

        public SmtpEmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Send(string to, string subject, string html)
        {
            using var client = new SmtpClient(Host, Port);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = Credentials;
            client.Send("noreply@orkad.fr", to, subject, html);
        }

        protected string Host => configuration.GetRequiredValue(CONFIG_SMTP_HOST);
        protected int Port => configuration.GetRequiredIntValue(CONFIG_SMTP_PORT);
        protected NetworkCredential Credentials => new NetworkCredential(configuration.GetRequiredValue(CONFIG_SMTP_USERNAME), configuration.GetRequiredValue(CONFIG_SMTP_PASSWORD));
    }
}
