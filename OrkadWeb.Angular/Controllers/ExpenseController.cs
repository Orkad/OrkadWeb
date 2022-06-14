using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.Expenses.Commands;
using OrkadWeb.Logic.Expenses.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    public class ExpenseController : ApiController
    {
        private readonly IMediator mediator;

        public ExpenseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<AddExpenseCommand.Result> Add(AddExpenseCommand command) => await mediator.Send(command);

        [HttpGet]
        public async Task<GetExpensesQuery.Result> GetAll() => await mediator.Send(new GetExpensesQuery());

        [HttpGet]
        public async Task<GetMonthlyExpensesQuery.Result> GetMonthly([FromQuery] DateTime month) => await mediator.Send(new GetMonthlyExpensesQuery() { Month = month });
    }
}
