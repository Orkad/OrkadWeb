using Microsoft.Extensions.Configuration;
using OrkadWeb.Common;
using System.Net;
using System.Net.Mail;

namespace OrkadWeb.Angular.Config
{
    public class SmtpConfig : ISmtpClientProvider
    {
        public const string CONFIG_SMTP_HOST = "Smtp:Host";
        public const string CONFIG_SMTP_PORT = "Smtp:Port";
        public const string CONFIG_SMTP_USERNAME = "Smtp:Username";
        public const string CONFIG_SMTP_PASSWORD = "Smtp:Password";

        private readonly IConfiguration configuration;

        public SmtpConfig(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected string Host => configuration.GetRequiredValue(CONFIG_SMTP_HOST);
        protected int Port => configuration.GetRequiredIntValue(CONFIG_SMTP_PORT);
        protected NetworkCredential Credentials => new NetworkCredential(configuration.GetRequiredValue(CONFIG_SMTP_USERNAME), configuration.GetRequiredValue(CONFIG_SMTP_PASSWORD));

        public SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient(Host, Port);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = Credentials;
            return client;
        }
    }
}
