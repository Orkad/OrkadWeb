using FluentValidation;
using MediatR;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Logic.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Shares.Commands.AddExpenseOnShare
{
    public class AddExpenseHandler : IRequestHandler<AddExpenseCommand>
    {
        private readonly IDataService dataService;
        private readonly IAuthenticatedUser authenticatedUser;

        public AddExpenseHandler(IDataService dataService, IAuthenticatedUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<Unit> Handle(AddExpenseCommand command, CancellationToken cancellationToken)
        {
            UserShare userShare = null;
            if (command.ShareId.HasValue)
            {
                userShare = dataService.Query<UserShare>().Single(us => us.User.Id == authenticatedUser.Id && us.Share.Id == command.ShareId);
            }
            await dataService.InsertAsync(new Expense
            {
                Amount = command.Amount,
                Date = DateTime.Now,
                Name = command.Name,
                UserShare = userShare,
            });
            return Unit.Value;
        }
    }
}
