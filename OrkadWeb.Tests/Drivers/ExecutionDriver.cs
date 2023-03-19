using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Drivers
{
    [Binding]
    public class ExceptionContext
    {
        private Exception? exception;

        public Exception? HandleException()
        {
            var exception = this.exception;
            this.exception = null;
            return exception;
        }

        public void SetException(Exception exception)
        {
            this.exception = exception;
        }
    }

    public sealed class ExceptionCatchPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly ExceptionContext exceptionContext;

        public ExceptionCatchPipeline(ExceptionContext exceptionContext)
        {
            this.exceptionContext = exceptionContext;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                exceptionContext.SetException(ex);
            }
            return default;
        }
    }
}
