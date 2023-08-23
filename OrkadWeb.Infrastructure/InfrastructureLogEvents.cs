using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrkadWeb.Infrastructure
{
    public static partial class InfrastructureLogEvents
    {
        [LoggerMessage(
            EventId = 2000,
            Level = LogLevel.Debug,
            Message = "[QUERY] [{query} {data}] Done")]
        public static partial void LogQueryDone(this ILogger logger, string query, string data);

        [LoggerMessage(
            EventId = 2001,
            Level = LogLevel.Error,
            Message = "[QUERY] [{query} {data}] Failed")]
        public static partial void LogQueryFailed(this ILogger logger, string query, string data);

        [LoggerMessage(
            EventId = 2002,
            Level = LogLevel.Information,
            Message = "[COMMAND] [{command} {data}] Done")]
        public static partial void LogCommandDone(this ILogger logger, string command, string data);

        [LoggerMessage(
            EventId = 2003,
            Level = LogLevel.Error,
            Message = "[COMMAND] [{command} {data}] Failed")]
        public static partial void LogCommandFailed(this ILogger logger, string command, string data);
    }
}
