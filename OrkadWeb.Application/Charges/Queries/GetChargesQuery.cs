using NHibernate.Linq;
using OrkadWeb.Application.Charges.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.Charges.Queries;

/// <summary>
/// Get all monthly charges defined by the authenticated user
/// </summary>
public class GetChargesQuery : IQuery<IEnumerable<ChargeDto>>
{
    public class Handler : IQueryHandler<GetChargesQuery, IEnumerable<ChargeDto>>
    {
        private readonly IDataService dataService;
        private readonly IAppUser authenticatedUser;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<IEnumerable<ChargeDto>> Handle(GetChargesQuery request, CancellationToken cancellationToken)
        {
            var query = dataService.Query<Charge>()
                .Where(mt => mt.Owner.Id == authenticatedUser.Id);
            return await query.Select(mt => new ChargeDto
            {
                Id = mt.Id,
                Amount = mt.Amount,
                Name = mt.Name,
            }).ToListAsync(cancellationToken);
        }
    }
}