using NHibernate.Linq;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.MonthlyTransactions.Models;
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
    public class GetIncomesQuery : IQuery<IEnumerable<MonthlyIncomeVM>>
    {
        public class Handler : IQueryHandler<GetIncomesQuery, IEnumerable<MonthlyIncomeVM>>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<IEnumerable<MonthlyIncomeVM>> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
            {
                var query = dataService.Query<MonthlyTransaction>()
                    .Where(mt => mt.Amount > 0) // Incomes
                    .Where(mt => mt.Owner.Id == authenticatedUser.Id);
                return await query.Select(mt => new MonthlyIncomeVM
                {
                    Id = mt.Id,
                    Amount = mt.Amount,
                    Name = mt.Name,
                }).ToListAsync(cancellationToken);
            }
        }
    }
}
