using NHibernate.Linq;
using OrkadWeb.Application.MonthlyTransactions.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.MonthlyTransactions.Queries
{
    /// <summary>
    /// Get all monthly charges defined by the authenticated user
    /// </summary>
    public class GetChargesQuery : IQuery<IEnumerable<MonthlyChargeVM>>
    {
        public class Handler : IQueryHandler<GetChargesQuery, IEnumerable<MonthlyChargeVM>>
        {
            private readonly IDataService dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IDataService dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<IEnumerable<MonthlyChargeVM>> Handle(GetChargesQuery request, CancellationToken cancellationToken)
            {
                var query = dataService.Query<Charge>()
                    .Where(mt => mt.Owner.Id == authenticatedUser.Id);
                return await query.Select(mt => new MonthlyChargeVM
                {
                    Id = mt.Id,
                    Amount = mt.Amount,
                    Name = mt.Name,
                }).ToListAsync(cancellationToken);
            }
        }
    }
}
