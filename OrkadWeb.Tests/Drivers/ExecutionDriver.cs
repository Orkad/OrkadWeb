using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Drivers
{
    [Binding]
    public class ExecutionDriver
    {
        private readonly IMediator mediator;

        public ExecutionDriver(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Exception? Exception { get; set; }

        public async Task Send(IRequest<Unit> request)
        {
            try
            {
                await mediator.Send(request);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
    }
}
