using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.Expenses.Queries;

public class GetExpensesQuery : IQuery<GetExpensesQuery.Result>
{
    public class Result
    {
        public class Row
        {
            public int Id { get; set; }
            public DateTime Date { get; init; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }

        public List<Row> Rows { get; set; }
    }

    public class Handler : IRequestHandler<GetExpensesQuery, Result>
    {
        private readonly IDataService dataService;
        private readonly IAppUser authenticatedUser;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<Result> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var query = dataService.Query<Transaction>()
                .Where(t => t.Owner.Id == authenticatedUser.Id && t.Amount > 0)
                .Select(t => new Result.Row
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Name = t.Name,
                }).OrderByDescending(r => r.Date);
            var results = await query.ToListAsync(cancellationToken);
            return new Result
            {
                Rows = results,
            };
        }
    }
}