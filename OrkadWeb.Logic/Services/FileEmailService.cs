using OrkadWeb.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Services
{
    /// <summary>
    /// Fake email service that write email into files
    /// </summary>
    public class FileEmailService : IEmailService
    {
        public void Send(string to, string subject, string html) => SendAsync(to, subject, html).RunSynchronously();

        public async Task SendAsync(string to, string subject, string html, CancellationToken cancellationToken = default)
        {
            if (Directory.Exists("Emails"))
            {
                Directory.CreateDirectory("Emails");
            }
            using var writer = File.CreateText($"Emails/{DateTime.Now:yyyyMMddTHHmmss}_{to}.email");
            await writer.WriteLineAsync($"to: {to}");
            await writer.WriteLineAsync($"Subject: {subject}");
            await writer.WriteLineAsync("Message:");
            await writer.WriteLineAsync(html);
        }
    }
}
