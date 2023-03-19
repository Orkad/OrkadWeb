using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Common.Interfaces
{
    /// <summary>
    /// Service that handle mail delivery.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an html email.
        /// </summary>
        /// <param name="to">Recipient of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="html">Html content of the email.</param>
        void Send(string to, string subject, string html);

        /// <summary>
        /// Send an html email
        /// </summary>
        /// <param name="to">Recipient of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="html">Html content of the email.</param>
        /// <param name="cancellationToken">a token that could cancel the operation</param>
        Task SendAsync(string to, string subject, string html, CancellationToken cancellationToken = default);
    }
}
