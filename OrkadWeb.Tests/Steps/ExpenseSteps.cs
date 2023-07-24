using NHibernate.Linq;
using OrkadWeb.Application.Expenses.Commands;
using System;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class ExpenseSteps
    {
        private readonly ISender sender;
        private readonly IDataService dataService;

        public ExpenseSteps(ISender sender, IDataService dataService)
        {
            this.sender = sender;
            this.dataService = dataService;
        }

        [When(@"j'ajoute la dépense de (.*)€ à la date du (.*) que j'appelle (.*)")]
        public async Task WhenJAjouteLaDepenseDeALaDateDuQueJAppelle(decimal amount, DateTime date, string name)
        {
            await sender.Send(new AddExpenseCommand
            {
                Amount = amount,
                Date = date,
                Name = name,
            });
        }

        [Then(@"j'ai une dépense de (.*)€ à la date du (.*) qui s'appelle (.*)")]
        public async Task ThenJaiLaDepense(decimal amount, DateTime date, string name)
        {
            var transaction = await dataService.Query<Transaction>()
                .Where(t => t.Date == date)
                .Where(t => t.Amount == amount)
                .Where(t => t.Name == name)
                .SingleOrDefaultAsync();
            Check.That(transaction).IsNotNull();
        }
    }
}
