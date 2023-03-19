using MediatR;
using OrkadWeb.Tests.Contexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Tests.Drivers
{
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
