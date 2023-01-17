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
    [Route("api/monthly")]
    public class MonthlyTransactionController : ApiController
    {
        public MonthlyTransactionController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [Route("charges")]
        public async Task<IEnumerable<MonthlyChargeVM>> GetCharges() => await Query(new GetChargesQuery());

        [HttpPost]
        [Route("charges")]
        public async Task<int> AddCharge(AddChargeCommand command) => await Command(command);

        [HttpPut]
        [Route("charges/{id}")]
        public async Task EditCharge(EditChargeCommand command) => await Command(command);

        [HttpDelete]
        [Route("charges/{id}")]
        public async Task DeleteCharge(int id) => await Command(new DeleteChargeCommand { Id = id });

        [HttpGet]
        [Route("incomes")]
        public async Task<IEnumerable<MonthlyIncomeVM>> GetIncomes() => await Query(new GetIncomesQuery());

        [HttpPost]
        [Route("incomes")]
        public async Task<int> AddIncome(AddIncomeCommand command) => await Command(command);

        [HttpPut]
        [Route("incomes/{id}")]
        public async Task EditIncome(EditIncomeCommand command) => await Command(command);

        [HttpDelete]
        [Route("incomes/{id}")]
        public async Task DeleteIncome(int id) => await Command(new DeleteIncomeCommand { Id = id });

    }
}
