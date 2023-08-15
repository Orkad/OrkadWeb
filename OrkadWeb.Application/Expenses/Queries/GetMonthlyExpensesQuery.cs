using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.Expenses.Queries;

public class GetMonthlyExpensesQuery : IQuery<GetMonthlyExpensesQuery.Result>
{
    public DateTime Month { get; init; }

    public class Result
    {
        public class Row
        {
            public int Id { get; set; }
            public DateTime Date { get; init; }
            public string Name { get; init; }
            public decimal Amount { get; init; }
        }

        public List<Row> Rows { get; init; }
    }

    public class Handler : IQueryHandler<GetMonthlyExpensesQuery, Result>
    {
        private readonly IDataService dataService;
        private readonly IAppUser authenticatedUser;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<Result> Handle(GetMonthlyExpensesQuery query, CancellationToken cancellationToken)
        {
            var firstDay = new DateTime(query.Month.Year, query.Month.Month, 1);
            var lastDay = firstDay.AddMonths(1);
            var q = dataService.Query<Transaction>()
                .Where(t => t.Owner.Id == authenticatedUser.Id && t.Amount < 0 && firstDay <= t.Date && t.Date < lastDay)
                .Select(t => new Result.Row
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Name = t.Name,
                }).OrderByDescending(r => r.Date);
            return new Result
            {
                Rows = await q.ToListAsync(),
            };
        }
    }
}