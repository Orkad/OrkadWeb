﻿using OrkadWeb.Application.Common.Exceptions;

namespace OrkadWeb.Application.MonthlyTransactions.Commands;

public class EditIncomeCommand : ICommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }

    public class Validator : AbstractValidator<EditIncomeCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class Handler : ICommandHandler<EditIncomeCommand>
    {
        private readonly IAppUser authenticatedUser;
        private readonly IDataService dataService;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task Handle(EditIncomeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = await dataService.GetAsync<Income>(command.Id, cancellationToken);
            authenticatedUser.MustOwns(income);
            income.Name = command.Name;
            income.Amount = command.Amount;
            await context.SaveChanges(cancellationToken);
        }
    }
}