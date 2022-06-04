using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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

        public void Run(IRequest<Unit> request)
        {
            try
            {
                mediator.Send(request);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
    }
}
