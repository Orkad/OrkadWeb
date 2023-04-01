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
[Route("api/monthly/charges")]
public class MonthlyChargeController : ControllerBase
{
    private readonly ISender sender;

    public MonthlyChargeController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<MonthlyChargeVM>> GetCharges(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetChargesQuery(), cancellationToken);
    }

    [HttpPost]
    [Route("")]
    public async Task<int> AddCharge([FromBody] MonthlyChargeVM vm, CancellationToken cancellationToken)
    {
        return await sender.Send(new AddChargeCommand
        {
            Amount = vm.Amount,
            Name = vm.Name
        }, cancellationToken);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task EditCharge(int id, [FromBody] MonthlyChargeVM vm, CancellationToken cancellationToken)
    {
        if (id <= 0 || vm?.Id != id) throw new BadHttpRequestException("identifiant incorrect");
        await sender.Send(new EditChargeCommand
        {
            Id = id,
            Amount = vm.Amount,
            Name = vm.Name
        }, cancellationToken);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task DeleteCharge(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteChargeCommand { Id = id }, cancellationToken);
    }
}