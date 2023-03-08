using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Tests.Models;
using System.Collections.Generic;

namespace OrkadWeb.Tests.Contexts
{
    public class EmailTestService : IEmailService
    {
        private readonly LastContext lastContext;

        public EmailTestService(LastContext lastContext)
        {
            this.lastContext = lastContext;
        }
        public List<SendedEmail> SendedEmails { get; set; } = new List<SendedEmail>();
        public void Send(string to, string subject, string html)
        {
            var email = new SendedEmail
            {
                To = to,
                Subject = subject,
                Html = html,
            };
            lastContext.Mention(email);
            SendedEmails.Add(email);
        }
    }
}
