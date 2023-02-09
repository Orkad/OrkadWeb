using OrkadWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Tests.Contexts
{
    public class EmailContext : IEmailService
    {
        public List<SendedEmail> SendedEmails { get; set; } = new List<SendedEmail>();
        public void Send(string to, string subject, string html)
        {
            SendedEmails.Add(new SendedEmail
            {
                To = to,
                Subject = subject,
                Html = html,
            });
        }

        public class SendedEmail
        {
            public string To { get; init; }
            public string Subject { get; init; }
            public string Html { get; init; }
        }
    }
}
