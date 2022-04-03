using MediatR;
using OrkadWeb.Common;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Users.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IDataService dataService;

        public RegisterHandler(IDataService dataService)
        {
            this.dataService = dataService;
        }
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await dataService.InsertAsync(new User
            {
                Email = request.Email,
                Username = request.UserName,
                Password = Hash.Create(request.Password),
            });
            return Unit.Value;
        }
    }
}
