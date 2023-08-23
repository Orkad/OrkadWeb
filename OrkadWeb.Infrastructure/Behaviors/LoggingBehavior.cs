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
        catch (Exception)
        {
            if (isQuery)
            {
                logger.LogQueryFailed(requestName, json);
            }
            else // isCommand
            {
                logger.LogCommandFailed(requestName, json);
            }
            throw;
        }
    }
}
