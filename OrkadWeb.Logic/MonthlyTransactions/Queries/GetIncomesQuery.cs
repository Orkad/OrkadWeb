using NHibernate.Linq;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.MonthlyTransactions.Queries
{
    /// <summary>
    /// Get all monthly charges defined by the authenticated user
    /// </summary>
    public class GetIncomesQuery : IQuery<GetIncomesQuery.Result>
    {
        public class Result
        {
            public class Item
            {
                public int Id { get; set; }
                public decimal Amount { get; set; }
                public string Name { get; set; }
            }
            public List<Item> Items { get; set; }
        }

        public class Handler : IQueryHandler<GetIncomesQuery, Result>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Result> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
            {
                var query = dataService.Query<MonthlyTransaction>()
                    .Where(mt => mt.Amount > 0) // Incomes
                    .Where(mt => mt.Owner.Id == authenticatedUser.Id);
                var project = query.Select(mt => new Result.Item
                {
                    Id = mt.Id,
                    Amount = mt.Amount,
                    Name = mt.Name,
                });
                return new Result
                {
                    Items = await project.ToListAsync(cancellationToken),
                };
            }
        }
    }
}
