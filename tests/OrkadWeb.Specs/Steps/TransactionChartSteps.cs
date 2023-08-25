using OrkadWeb.Application.Transactions.Models;
using OrkadWeb.Application.Transactions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class TransactionChartSteps
    {
        private readonly ISender sender;
        private DateTime monthParam;
        private List<TransactionChartPoint> result;

        public TransactionChartSteps(ISender sender)
        {
            this.sender = sender;
        }

        [When(@"je récupère les données du tableau pour (.*)")]
        public async Task WhenJeRecupereLesDonneesDuTableauPour(DateTime date)
        {
            monthParam = date;
            result = await sender.Send(new GetTransactionChartDataQuery
            {
                Month = date,
            });
        }

        [Then(@"le tableau n'a aucune donnée")]
        public void ThenLeTableauNaAucuneDonnee()
        {
            Check.That(result).IsNotNull();
            Check.That(result).HasSize(1);
            Check.That(result.Single().X).IsEqualTo(monthParam);
            Check.That(result.Single().Y).IsEqualTo(0);
        }

        private record ExpectedChartRow(DateTime X, decimal Y);

        [Then(@"le tableau à les données suivantes")]
        public void ThenLeTableauALesDonneesSuivantes(Table table)
        {
            var expecteds = table.CreateSet<ExpectedChartRow>();
            var actuals = result.ToList();
            foreach (var expected in expecteds)
            {
                var actual = actuals.FirstOrDefault(a => a.X == expected.X && a.Y == expected.Y);
                Check.That(actual).IsNotNull();
                actuals.Remove(actual);
            }
            // check that every actual has been checked
            Check.That(actuals).IsEmpty();
        }

    }
}
