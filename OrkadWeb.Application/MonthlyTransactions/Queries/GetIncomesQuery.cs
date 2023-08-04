using NHibernate.Linq;
using OrkadWeb.Application.MonthlyTransactions.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.MonthlyTransactions.Queries
{
    /// <summary>
    /// Get all monthly charges defined by the authenticated user
    /// </summary>
    public class GetIncomesQuery : IQuery<IEnumerable<MonthlyIncomeVM>>
    {
        public class Handler : IQueryHandler<GetIncomesQuery, IEnumerable<MonthlyIncomeVM>>
        {
            private readonly IDataService dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IDataService dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<IEnumerable<MonthlyIncomeVM>> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
            {
                var query = dataService.Query<Income>()
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
