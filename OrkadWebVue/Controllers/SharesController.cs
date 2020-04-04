using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Services;
using System.Security.Claims;

namespace OrkadWebVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SharesController : ControllerBase
    {
        private readonly ShareService shareService;

        public SharesController(ShareService shareService)
        {
            this.shareService = shareService;
        }

        protected int ConnectedUserId
            => int.TryParse((User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.PrimarySid)?.Value, out int id) ? id : 0;

        [HttpGet]
        public dynamic Get()
        {
            return shareService.GetSharesForUser(ConnectedUserId);
        }

        [HttpGet("{id}")]
        public dynamic Get(int id)
        {
            return shareService.GetShareDetail(id);
        }
    }
}