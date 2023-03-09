using Microsoft.Extensions.Configuration;
using OrkadWeb.Domain.Extensions;
using System.Net;

namespace OrkadWeb.Infrastructure.Services
{
    public class SmtpConfig
    {
        public string Host { get; }
        public int Port { get; }
        public NetworkCredential Credentials { get; }

        public SmtpConfig(IConfiguration configuration)
        {
            Host = configuration.GetRequiredValue("Smtp:Host");
            Port = configuration.GetRequiredIntValue("Smtp:Port");
            var username = configuration.GetRequiredValue("Smtp:Username");
            var password = configuration.GetRequiredValue("Smtp:Password");
            Credentials = new NetworkCredential(username, password);
        }
    }
}
