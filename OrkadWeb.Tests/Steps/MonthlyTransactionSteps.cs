using OrkadWeb.Application.MonthlyTransactions.Models;
using OrkadWeb.Application.MonthlyTransactions.Queries;
using OrkadWeb.Application.Users;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class MonthlyTransactionSteps
    {
        private readonly IDataService dataService;
        private readonly IAppUser user;
        private readonly ISender sender;
        private IEnumerable<MonthlyChargeVM> charges;
        private IEnumerable<MonthlyIncomeVM> incomes;

        public MonthlyTransactionSteps(IDataService dataService, IAppUser user, ISender sender)
        {
            this.dataService = dataService;
            this.user = user;
            this.sender = sender;
        }

        [Given(@"il existe une charge mensuelle (.*) d'un montant de (.*)€")]
        public void GivenIlExisteUneChargeMensuelleLoyerDunMontantDe(string name, int amount)
        {
            var entity = new Charge
            {
                Name = name,
                Amount = amount,
                Owner = dataService.Load<User>(user.Id),
            };
            dataService.Insert(entity);
        }

        [Given(@"il existe un revenu mensuel (.*) d'un montant de (.*)€")]
        public void GivenIlExisteUnRevenuMensuelDunMontantDe(string name, int amount)
        {
            var entity = new Income
            {
                Name = name,
                Amount = amount,
                Owner = dataService.Load<User>(user.Id),
            };
            dataService.Insert(entity);
        }

        [When(@"j'affiche le budget mensuel")]
        public async Task WhenJafficheLaListeDesDepensesMensuelles()
        {
            charges = await sender.Send(new GetChargesQuery());
            incomes = await sender.Send(new GetIncomesQuery());
        }

        [Then(@"il y a les charges mensuelles suivantes")]
        public void ThenIlYALesChargesMensuellesSuivantes(Table table)
        {
            foreach (var row in table.Rows)
            {
                var expectedName = row.GetString("Libellé");
                var expectedAmount = decimal.Parse(row.GetString("Montant").Replace("€", ""));
                Check.That(charges).HasElementThatMatches(c => c.Name == expectedName && c.Amount == expectedAmount);
            }
        }

        [Then(@"il y a les revenus mensuels suivants")]
        public void ThenIlYALesRevenusMensuelsSuivants(Table table)
        {
            foreach (var row in table.Rows)
            {
                var expectedName = row.GetString("Libellé");
                var expectedAmount = decimal.Parse(row.GetString("Montant").Replace("€", ""));
                Check.That(incomes).HasElementThatMatches(c => c.Name == expectedName && c.Amount == expectedAmount);
            }
        }

    }
}
