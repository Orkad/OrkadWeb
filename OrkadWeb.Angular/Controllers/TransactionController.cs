using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Expenses.Commands;
using OrkadWeb.Application.Transactions.Commands;
using OrkadWeb.Application.Transactions.Models;
using OrkadWeb.Application.Transactions.Queries;

namespace OrkadWeb.Angular.Controllers;

[ApiController]
[Authorize]
[Route("api/transactions/[action]")]
public class TransactionController : ControllerBase
{
    private readonly ISender sender;

    public TransactionController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    public async Task<List<TransactionVM>> GetMonthly([FromQuery] DateTime month,
        CancellationToken cancellationToken)
    {
        return await sender.Send(new GetTransactionsQuery { Month = month }, cancellationToken);
    }

    [HttpGet]
    public async Task<List<TransactionChartPoint>> GetChartData([FromQuery] DateTime month,
        CancellationToken cancellationToken)
    {
        return await sender.Send(new GetTransactionChartDataQuery { Month = month }, cancellationToken);
    }

    [HttpPost]
    public async Task<AddTransactionExpenseCommand.Result> AddExpense(AddTransactionExpenseCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task UpdateExpense(UpdateTransactionExpenseCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task<int> AddGain(AddTransactionGainCommand command, CancellationToken cancellationToken)
        => await sender.Send(command, cancellationToken);

    [HttpPost]
    public async Task Delete([FromBody] int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTransactionCommand { Id = id }, cancellationToken);
    }
}