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
        private readonly ITimeProvider timeProvider;

        public Handler(IDataService dataService, IAppUser appUser, ITimeProvider timeProvider)
        {
            this.dataService = dataService;
            this.appUser = appUser;
            this.timeProvider = timeProvider;
        }

        public async Task<List<TransactionChartPoint>> Handle(GetTransactionChartDataQuery query, CancellationToken cancellationToken)
        {
            var year = query.Month.Year;
            var month = query.Month.Month;
            var test = await dataService.Query<Charge>().Where(c => c.Owner.Id == appUser.Id)
                .ToListAsync(cancellationToken);
            var totalCharges = await dataService.Query<Charge>()
                .Where(c => c.Owner.Id == appUser.Id)
                .SumAsync(c => (decimal?)c.Amount, cancellationToken) ?? 0m;
            var totalIncomes = await dataService.Query<Income>()
                .Where(c => c.Owner.Id == appUser.Id)
                .SumAsync(c => (decimal?)c.Amount, cancellationToken) ?? 0m;
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

            var isCurrentMonth = year == timeProvider.Now.Year && month == timeProvider.Now.Month;
            if (!isCurrentMonth)
            {
                var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                var lastY = points.Last().Y;
                points.Add(new TransactionChartPoint
                {
                    X = lastDay,
                    Y = amount,
                });
            }

            return points;
        }
    }
}