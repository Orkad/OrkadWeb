using FluentValidation;
using MediatR;
using NHibernate.Linq;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Shares.Queries.GetMonthlyTotalExpenses
{
    public class GetMonthlyTotalExpensesHandler : IRequestHandler<GetMonthlyTotalExpensesQuery, decimal>
    {
        private readonly IDataService dataService;

        public GetMonthlyTotalExpensesHandler(IDataService dataService)
        {
            this.dataService = dataService;
        }
        public async Task<decimal> Handle(GetMonthlyTotalExpensesQuery query, CancellationToken cancellationToken)
        {
            return await dataService.Query<Transaction>().Where(e => e.Date.Year == query.Month.Year && e.Date.Month == query.Month.Month).SumAsync(e => e.Amount);
        }
    }
}
