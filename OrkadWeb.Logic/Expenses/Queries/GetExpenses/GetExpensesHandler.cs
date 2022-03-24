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
            var query = dataService.Query<Expense>()
                .Where(e => e.Owner.Id == authenticatedUser.Id)
                .Select(e => new GetExpensesResult.ExpenseRow
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    Date = e.Date,
                    Name = e.Name,
                }).OrderByDescending(r => r.Date);
            var results = await query.ToListAsync(cancellationToken);
            return new GetExpensesResult
            {
                Rows = results,
            };
        }
    }
}
