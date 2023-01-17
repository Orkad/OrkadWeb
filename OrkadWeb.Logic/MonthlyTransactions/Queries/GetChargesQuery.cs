using NHibernate.Linq;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.MonthlyTransactions.Models;
using OrkadWeb.Logic.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.MonthlyTransactions.Queries
{
    /// <summary>
    /// Get all monthly charges defined by the authenticated user
    /// </summary>
    public class GetChargesQuery : IQuery<IEnumerable<MonthlyChargeVM>>
    {
        public class Handler : IQueryHandler<GetChargesQuery, IEnumerable<MonthlyChargeVM>>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<IEnumerable<MonthlyChargeVM>> Handle(GetChargesQuery request, CancellationToken cancellationToken)
            {
                var query = dataService.Query<MonthlyTransaction>()
                    .Where(mt => mt.Amount < 0) // Charges
                    .Where(mt => mt.Owner.Id == authenticatedUser.Id);
                return await query.Select(mt => new MonthlyChargeVM
                {
                    Id = mt.Id,
                    Amount = -mt.Amount,
                    Name = mt.Name,
                }).ToListAsync(cancellationToken);
            }
        }
    }
}
