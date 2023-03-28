namespace OrkadWeb.Application.Expenses.Commands
{
    public class DeleteExpenseCommand : ICommand
    {
        public int Id { get; set; }

        public class Handler : ICommandHandler<DeleteExpenseCommand>
        {
            private readonly IDataService dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IDataService dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
            {
                using var context = dataService.Context();
                var transaction = await dataService.GetAsync<Transaction>(request.Id, cancellationToken);
                authenticatedUser.MustOwns(transaction);
                await dataService.DeleteAsync(transaction, cancellationToken);
                await context.SaveChanges(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
