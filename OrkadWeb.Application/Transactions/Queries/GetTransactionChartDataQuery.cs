using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using OrkadWeb.Application.Transactions.Models;

namespace OrkadWeb.Application.Transactions.Queries;

public class GetTransactionChartDataQuery : IQuery<List<TransactionChartPoint>>
{
    public DateTime Month { get; init; }

    class Handler : IQueryHandler<GetTransactionChartDataQuery, List<TransactionChartPoint>>
    {
        private readonly IDataService dataService;
        private readonly IAppUser appUser;

        public Handler(IDataService dataService, IAppUser appUser)
        {
            this.dataService = dataService;
            this.appUser = appUser;
        }
        
        public async Task<List<TransactionChartPoint>> Handle(GetTransactionChartDataQuery query, CancellationToken cancellationToken)
        {
            var year = query.Month.Year;
            var month = query.Month.Month;
            var test = await dataService.Query<Charge>().Where(c => c.Owner.Id == appUser.Id)
                .ToListAsync(cancellationToken);
            var totalCharges = await dataService.Query<Charge>()
                .Where(c => c.Owner.Id == appUser.Id)
                .SumAsync(c => c.Amount, cancellationToken);
            var totalIncomes = await dataService.Query<Income>()
                .Where(c => c.Owner.Id == appUser.Id)
                .SumAsync(c => c.Amount, cancellationToken);
            var transactionPerDays = await dataService.Query<Transaction>()
                .Where(t => t.Owner.Id == appUser.Id)
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .GroupBy(t => t.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Amount = g.Sum(t => t.Amount),
                }).OrderBy(r => r.Date)
                .ToListAsync(cancellationToken);
            var points = new List<TransactionChartPoint>();
            var amount = totalIncomes - totalCharges;
            points.Add(new TransactionChartPoint
            {
                X = new DateTime(year, month, 1),
                Y = amount,
            });
            foreach (var dailyTransactions in transactionPerDays)
            {
                amount += dailyTransactions.Amount;
                points.Add(new TransactionChartPoint
                {
                    X = dailyTransactions.Date,
                    Y = amount,
                });
            }

            return points;
        }
    }
}