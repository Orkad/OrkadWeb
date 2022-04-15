using MediatR;
using NHibernate.Linq;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Expenses.Queries.GetExpenses
{
    public class GetExpensesHandler : IRequestHandler<GetExpensesQuery, GetExpensesResult>
    {
        private readonly IDataService dataService;
        private readonly IAuthenticatedUser authenticatedUser;

        public GetExpensesHandler(IDataService dataService, IAuthenticatedUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<GetExpensesResult> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var query = dataService.Query<Transaction>()
                .Where(t => t.Owner.Id == authenticatedUser.Id && t.Amount > 0)
                .Select(t => new GetExpensesResult.ExpenseRow
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Name = t.Name,
                }).OrderByDescending(r => r.Date);
            var results = await query.ToListAsync(cancellationToken);
            return new GetExpensesResult
            {
                Rows = results,
            };
        }
    }
}
