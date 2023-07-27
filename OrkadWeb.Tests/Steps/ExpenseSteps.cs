using NHibernate.Linq;
using OrkadWeb.Application.Expenses.Commands;
using System;
using OrkadWeb.Application.Expenses.Queries;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class ExpenseSteps
    {
        private readonly ISender sender;
        private readonly IDataService dataService;
        private GetMonthlyExpensesQuery.Result displayExpensesResult;

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

        [Given(@"les dépenses suivantes")]
        public void GivenLesDepensesSuivantes(Table table)
        {
            var transactions = table.CreateSet<Transaction>(row => new Transaction
            {
                Amount = row.GetDecimal("montant"),
                Date = row.GetDateTime("date"),
                Name = row.GetString("nom"),
                Owner = dataService.Query<User>()
                        .Single(u => u.Username == row.GetString("propriétaire"))
            });
            foreach (var transaction in transactions)
            {
                dataService.Insert(transaction);
            }
        }

        [When(@"j'affiche la liste de mes dépenses sur le mois de (.*)")]
        public async Task WhenJafficheLaListeDeMesDepensesSurLeMoisDeJuillet(DateTime month)
        {
            displayExpensesResult = await sender.Send(new GetMonthlyExpensesQuery
            {
                Month = month,
            });
        }

        [Then(@"mes dépenses sont les suivantes")]
        public void ThenMesDepensesSontLesSuivantes(Table table)
        {
            var expecteds = table.CreateSet<Transaction>(row => new Transaction
            {
                Amount = row.GetDecimal("montant"),
                Date = row.GetDateTime("date"),
                Name = row.GetString("nom"),
            }).ToList();
            var displayedRows = displayExpensesResult.Rows;
            Check.That(displayedRows).HasSize(expecteds.Count);
            foreach (var expected in expecteds)
            {
                var actual = displayedRows.Single(r => r.Name == expected.Name);
                Check.That(actual.Amount).IsEqualTo(expected.Amount);
                Check.That(actual.Date).IsEqualTo(expected.Date);
            }
        }

        [Then(@"il n'y aucune dépense affichée")]
        public void ThenIlNyAucuneDepenseAffichee()
        {
            Check.That(displayExpensesResult.Rows).IsEmpty();
        }

        [When(@"je modifie la dépense ""(.*)"" par")]
        public void WhenJeModifieLaDepensePar(string name, Table table)
        {
            var dto = table.CreateSet<Transaction>(row => new Transaction
            {
                Amount = row.GetDecimal("montant"),
                Date = row.GetDateTime("date"),
                Name = row.GetString("nom"),
            }).Single();
            var id = dataService.Query<Transaction>().Single(t => t.Name == name).Id;
            sender.Send(new UpdateExpenseCommand
            {
                Id = id,
                Amount = dto.Amount,
                Date = dto.Date,
                Name = dto.Name,
            });
        }

        [When(@"je supprime la dépense ""(.*)""")]
        public void WhenJeSupprimeLaDepense(string name)
        {
            var id = dataService.Query<Transaction>().Single(t => t.Name == name).Id;
            sender.Send(new DeleteExpenseCommand()
            {
                Id = id,
            });
        }
    }
}
