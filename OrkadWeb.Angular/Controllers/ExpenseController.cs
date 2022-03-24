using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.Expenses.Commands.AddExpense;
using OrkadWeb.Logic.Expenses.Queries.GetExpenses;
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
        public async Task<AddExpenseResult> Add(AddExpenseCommand command) => await mediator.Send(command);

        [HttpGet]
        public async Task<List<GetExpensesResult.ExpenseRow>> GetAll() => (await mediator.Send(new GetExpensesQuery())).Rows;
    }
}
