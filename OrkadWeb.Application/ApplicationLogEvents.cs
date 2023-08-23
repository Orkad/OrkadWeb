using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrkadWeb.Application
{
    public static partial class ApplicationLogEvents
    {
        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Information,
            Message = "user {Username} successfully authenticated")]
        internal static partial void LogAuthenticationSuccess(this ILogger logger, string username);

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Information,
            Message = "user {Username} failed to authenticate")]
        internal static partial void LogAuthenticationFailed(this ILogger logger, string username);

        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Information,
            Message = "email {Email} of user {Username} is validated")]
        internal static partial void LogEmailValidationSucess(this ILogger logger, string email, string username);

        [LoggerMessage(
            EventId = 1004,
            Level = LogLevel.Warning,
            Message = "email {Email} of user {Username} failed: {Reason}")]
        internal static partial void LogEmailValidationFail(this ILogger logger, string email, string username, string reason);
    }
}
