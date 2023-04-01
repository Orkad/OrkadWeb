using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.MonthlyTransactions.Commands;
using OrkadWeb.Application.MonthlyTransactions.Models;
using OrkadWeb.Application.MonthlyTransactions.Queries;

namespace OrkadWeb.Angular.Controllers;

[ApiController]
[Authorize]
[Route("api/monthly/incomes")]
public class MonthlyIncomeController : ControllerBase
{
    private readonly ISender sender;

    public MonthlyIncomeController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<MonthlyIncomeVM>> GetIncomes(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetIncomesQuery(), cancellationToken);
    }

    [HttpPost]
    [Route("")]
    public async Task<int> AddIncome([FromBody] MonthlyIncomeVM vm, CancellationToken cancellationToken)
    {
        return await sender.Send(new AddIncomeCommand
        {
            Amount = vm.Amount,
            Name = vm.Name
        }, cancellationToken);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task EditIncome(int id, [FromBody] MonthlyIncomeVM vm, CancellationToken cancellationToken)
    {
        if (id <= 0 || vm?.Id != id) throw new BadHttpRequestException("identifiant incorrect");
        await sender.Send(new EditIncomeCommand
        {
            Id = id,
            Amount = vm.Amount,
            Name = vm.Name
        }, cancellationToken);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task DeleteIncome(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteIncomeCommand { Id = id }, cancellationToken);
    }
}