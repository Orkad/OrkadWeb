using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        public async Task<GetExpensesQuery.Result> GetAll() => await Query(new GetExpensesQuery());

        [HttpGet]
        public async Task<GetMonthlyExpensesQuery.Result> GetMonthly([FromQuery] DateTime month) => await Query(new GetMonthlyExpensesQuery() { Month = month });

        [HttpPost]
        public async Task<AddExpenseCommand.Result> Add(AddExpenseCommand command) => await Command(command);

        [HttpPost]
        public async Task Update(UpdateExpenseCommand command) => await Command(command);

        [HttpPost]
        public async Task Delete([FromBody] int id) => await Command(new DeleteExpenseCommand { Id = id });
    }
}
