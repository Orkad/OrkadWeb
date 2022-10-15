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
        [ActionName("charges")]
        public async Task<GetChargesQuery.Result> GetCharges() => await Query(new GetChargesQuery());

        [HttpDelete("{id}")]
        [ActionName("charges")]
        public async Task DeleteCharge(int id) => await Command(new DeleteChargeCommand { ChargeId = id });

        [HttpGet]
        [ActionName("incomes")]
        public async Task<GetIncomesQuery.Result> Incomes() => await Query(new GetIncomesQuery());
    }
}
