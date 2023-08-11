using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Specs.Contexts;

namespace OrkadWeb.Specs.Drivers
{
    public sealed class ExceptionCatchPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IBaseRequest
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
