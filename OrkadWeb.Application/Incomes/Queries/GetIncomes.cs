using NHibernate.Linq;
using OrkadWeb.Application.MonthlyTransactions.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Application.MonthlyTransactions.Queries;

/// <summary>
/// Get all monthly charges defined by the authenticated user
/// </summary>
public class GetIncomes : IQuery<IEnumerable<IncomeVM>>
{
    public class Handler(IDataService dataService, IAppUser authenticatedUser) : IQueryHandler<GetIncomes, IEnumerable<IncomeVM>>
    {
        public async Task<IEnumerable<IncomeVM>> Handle(GetIncomes request, CancellationToken cancellationToken)
        {
            return dataService.Query<Income>()
                .Where(mt => mt.Owner.Id == authenticatedUser.Id)
                .Select(i => new IncomeVM
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    Name = i.Name,
                });
        }
    }
}