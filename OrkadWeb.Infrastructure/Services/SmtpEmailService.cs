using Microsoft.Extensions.Configuration;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Extensions;
using System.Net;
using System.Net.Mail;

namespace OrkadWeb.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string host;
        private readonly int port;
        private readonly NetworkCredential credentials;

        public SmtpEmailService(string host, int port, string username, string password)
        {
            this.host = host;
            this.port = port;
            this.credentials = new NetworkCredential(username, password);
        }

        public void Send(string to, string subject, string html)
        {
            using var client = new SmtpClient(host, port);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            client.Send("noreply@orkad.fr", to, subject, html);
        }
    }
}
