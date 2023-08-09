namespace OrkadWeb.Application.Charges.Commands;

public class DeleteChargeCommand : ICommand
{
    public int Id { get; set; }

    public class Handler : ICommandHandler<DeleteChargeCommand>
    {
        private readonly IAppUser authenticatedUser;
        private readonly IDataService dataService;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task Handle(DeleteChargeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = await dataService.GetAsync<Charge>(command.Id, cancellationToken);
            authenticatedUser.MustOwns(transaction);
            await dataService.DeleteAsync(transaction, cancellationToken);
            await context.SaveChanges(cancellationToken);
        }
    }
}