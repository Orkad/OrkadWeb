using MediatR;
using NHibernate.Linq;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Application.Users;
using OrkadWeb.Domain.Common;
using OrkadWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Expenses.Queries
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
            private readonly IRepository dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IRepository dataService, IAppUser authenticatedUser)
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
