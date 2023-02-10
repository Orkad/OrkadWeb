using MediatR;
using NHibernate.Linq;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Expenses.Queries
{
    public class GetExpensesQuery : IQuery<GetExpensesQuery.Result>
    {
        public class Result
        {
            public class Row
            {
                public int Id { get; set; }
                public DateTime Date { get; set; }
                public string Name { get; set; }
                public decimal Amount { get; set; }
            }

            public List<Row> Rows { get; set; }
        }

        public class Handler : IRequestHandler<GetExpensesQuery, Result>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
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
}
