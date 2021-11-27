using MediatR;
using OrkadWeb.Data;
using OrkadWeb.Logic.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Users.Commands.Register
{
    public class RegisterCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterHandler : IRequestHandler<RegisterCommand, Response>
    {
        private readonly IDataService dataService;

        public RegisterHandler(IDataService dataService)
        {
            this.dataService = dataService;
        }
        public async Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
