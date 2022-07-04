using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.MonthlyTransactions.Queries;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    public class MonthlyTransactionController : ApiController
    {
        public MonthlyTransactionController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<GetMonthlyTransactionsQuery.Result> GetAll() => await Query(new GetMonthlyTransactionsQuery());
    }
}
