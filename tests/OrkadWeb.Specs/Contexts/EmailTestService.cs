﻿using OrkadWeb.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Specs.Models;

namespace OrkadWeb.Specs.Contexts
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

        public Task SendAsync(string to, string subject, string html, CancellationToken cancellationToken = default)
        {
            var email = new SendedEmail
            {
                To = to,
                Subject = subject,
                Html = html,
            };
            lastContext.Mention(email);
            SendedEmails.Add(email);
            return Task.CompletedTask;
        }
    }
}
