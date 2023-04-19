using MediatR;
using Microsoft.Extensions.Logging;
using OrkadWeb.Domain.Common;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var json = JsonSerializer.Serialize(request);
        var isQuery = request is IQuery<TResponse>;
        try
        {
            var response = await next();
            if (isQuery)
            {
                logger.LogQueryDone(requestName, json);
            }
            else // isCommand
            {
                logger.LogCommandDone(requestName, json);
            }
            return response;
        }
        catch (Exception ex)
        {
            if (isQuery)
            {
                logger.LogQueryFailed(requestName, json, ex);
            }
            else // isCommand
            {
                logger.LogCommandFailed(requestName, json, ex);
            }
            throw;
        }
    }
}

public static partial class LoggerMessageDefinitions
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "[QUERY] [{query} {data}] Done")]
    public static partial void LogQueryDone(this ILogger logger, string query, string data);

    [LoggerMessage(Level = LogLevel.Error, Message = "[QUERY] [{query} {data}] Failed: {exception}")]
    public static partial void LogQueryFailed(this ILogger logger, string query, string data, Exception exception);

    [LoggerMessage(Level = LogLevel.Information, Message = "[COMMAND] [{command} {data}] Done")]
    public static partial void LogCommandDone(this ILogger logger, string command, string data);

    [LoggerMessage(Level = LogLevel.Error, Message = "[COMMAND] [{command} {data}] Failed\r\n{exception}")]
    public static partial void LogCommandFailed(this ILogger logger, string command, string data, Exception exception);
}
