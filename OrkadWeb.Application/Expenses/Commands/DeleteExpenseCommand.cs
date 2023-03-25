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
                await dataService.TransactAsync(async () =>
                {
                    var transaction = await dataService.GetAsync<Transaction>(request.Id);
                    authenticatedUser.MustOwns(transaction);
                    await dataService.DeleteAsync(transaction);
                }, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
