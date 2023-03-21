using MediatR;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Common.Behaviors
{
    public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly IRepository dataService;

        public UnitOfWorkBehavior(IRepository dataService)
        {
            this.dataService = dataService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using (var unitOfWork = dataService.BeginUnitOfWork())
            {
                try
                {
                    var response = await next();
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    return response;
                }
                catch
                {
                    await unitOfWork.CancelChangesAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
