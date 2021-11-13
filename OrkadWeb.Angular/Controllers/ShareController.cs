using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.Shares.Queries.GetPersonalShares;
using OrkadWeb.Logic.Shares.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShareController : ControllerBase
    {
        private readonly Mediator mediator;

        public ShareController(Mediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<ShareItem>> PersonnalShares()
            => await mediator.Send(new GetPersonalSharesRequest()
            {
                UserId = 1,
            });
    }
}
