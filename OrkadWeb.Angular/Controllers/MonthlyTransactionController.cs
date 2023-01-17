using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.MonthlyTransactions.Commands;
using OrkadWeb.Logic.MonthlyTransactions.Models;
using OrkadWeb.Logic.MonthlyTransactions.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    public class MonthlyTransactionController : ApiController
    {
        public MonthlyTransactionController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [ActionName("charges")]
        public async Task<IEnumerable<MonthlyChargeVM>> GetCharges() => await Query(new GetChargesQuery());

        [HttpPost]
        [ActionName("charges")]
        public async Task<int> AddCharge(AddChargeCommand command) => await Command(command);

        [HttpPut("{id}")]
        [ActionName("charges")]
        public async Task EditCharge(EditChargeCommand command) => await Command(command);

        [HttpDelete("{id}")]
        [ActionName("charges")]
        public async Task DeleteCharge(int id) => await Command(new DeleteChargeCommand { Id = id });

        [HttpGet]
        [ActionName("incomes")]
        public async Task<GetIncomesQuery.Result> Incomes() => await Query(new GetIncomesQuery());
    }
}
