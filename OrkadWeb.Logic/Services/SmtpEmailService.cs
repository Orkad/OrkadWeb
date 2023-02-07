using OrkadWeb.Common;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly ISmtpClientProvider smtpClientProvider;

        public SmtpEmailService(ISmtpClientProvider smtpClientProvider)
        {
            this.smtpClientProvider = smtpClientProvider;
        }

        public void Send(string to, string subject, string html)
        {
            using var client = smtpClientProvider.GetSmtpClient();
            client.Send("noreply@orkad.fr", to, subject, html);
        }
    }
}
