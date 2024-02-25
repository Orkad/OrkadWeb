using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Charges.Commands;
using OrkadWeb.Application.Charges.Models;
using OrkadWeb.Application.Charges.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
    public async Task<IEnumerable<ChargeDto>> GetCharges(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetChargesQuery(), cancellationToken);
    }

    [HttpPost]
    [Route("")]
    public async Task<int> AddCharge([FromBody] ChargeDto vm, CancellationToken cancellationToken)
    {
        return await sender.Send(new AddChargeCommand
        {
            Amount = vm.Amount,
            Name = vm.Name
        }, cancellationToken);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task EditCharge(int id, [FromBody] ChargeDto vm, CancellationToken cancellationToken)
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