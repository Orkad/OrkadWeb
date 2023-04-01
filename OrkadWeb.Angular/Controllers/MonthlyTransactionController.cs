using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.MonthlyTransactions.Commands;
using OrkadWeb.Application.MonthlyTransactions.Models;
using OrkadWeb.Application.MonthlyTransactions.Queries;

namespace OrkadWeb.Angular.Controllers;

[Route("api/monthly")]
public class MonthlyTransactionController : ControllerBase
{
    private readonly ISender sender;

    public MonthlyTransactionController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    [Route("charges")]
    public async Task<IEnumerable<MonthlyChargeVM>> GetCharges(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetChargesQuery(), cancellationToken);
    }

    [HttpPost]
    [Route("charges")]
    public async Task<int> AddCharge(AddChargeCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    [HttpPut]
    [Route("charges/{id:int}")]
    public async Task EditCharge(EditChargeCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }

    [HttpDelete]
    [Route("charges/{id:int}")]
    public async Task DeleteCharge(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteChargeCommand { Id = id }, cancellationToken);
    }

    [HttpGet]
    [Route("incomes")]
    public async Task<IEnumerable<MonthlyIncomeVM>> GetIncomes(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetIncomesQuery(), cancellationToken);
    }

    [HttpPost]
    [Route("incomes")]
    public async Task<int> AddIncome(AddIncomeCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    [HttpPut]
    [Route("incomes/{id:int}")]
    public async Task EditIncome(EditIncomeCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }

    [HttpDelete]
    [Route("incomes/{id:int}")]
    public async Task DeleteIncome(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteIncomeCommand { Id = id }, cancellationToken);
    }
}