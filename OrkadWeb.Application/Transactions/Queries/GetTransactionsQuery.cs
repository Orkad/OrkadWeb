using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using OrkadWeb.Application.Transactions.Models;

namespace OrkadWeb.Application.Transactions.Queries;

public class GetTransactionsQuery : IQuery<List<TransactionVM>>
{
    /// <summary>
    /// Month criteria
    /// </summary>
    public DateTime Month { get; set; }

    class Handler : IQueryHandler<GetTransactionsQuery, List<TransactionVM>>
    {
        private readonly IDataService dataService;
        private readonly IAppUser appUser;

        public Handler(IDataService dataService, IAppUser appUser)
        {
            this.dataService = dataService;
            this.appUser = appUser;
        }
        
        public async Task<List<TransactionVM>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
        {
            return await dataService.Query<Transaction>()
                .Where(t => t.Owner.Id == appUser.Id)
                .Where(t => t.Date.Year == query.Month.Year && t.Date.Month == query.Month.Month)
                .Select(t => new TransactionVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Date = t.Date,
                    Amount = t.Amount,
                }).ToListAsync(cancellationToken);
        }
    }
}