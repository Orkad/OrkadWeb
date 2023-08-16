using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Expenses.Commands;
using OrkadWeb.Application.Expenses.Queries;
using OrkadWeb.Application.Transactions.Commands;

namespace OrkadWeb.Angular.Controllers;

[ApiController]
[Authorize]
[Route("api/expense/[action]")]
public class ExpenseController : ControllerBase
{
    private readonly ISender sender;

    public ExpenseController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    public async Task<GetExpensesQuery.Result> GetAll(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetExpensesQuery(), cancellationToken);
    }

    [HttpGet]
    public async Task<GetMonthlyExpensesQuery.Result> GetMonthly([FromQuery] DateTime month,
        CancellationToken cancellationToken)
    {
        return await sender.Send(new GetMonthlyExpensesQuery { Month = month }, cancellationToken);
    }

    [HttpPost]
    public async Task<AddTransactionExpenseCommand.Result> Add(AddTransactionExpenseCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task Update(UpdateTransactionExpenseCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task Delete([FromBody] int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTransactionCommand { Id = id }, cancellationToken);
    }
}