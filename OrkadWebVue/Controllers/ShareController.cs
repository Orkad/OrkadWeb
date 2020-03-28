using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Services;

namespace OrkadWebVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly ShareService shareService;

        public ShareController(ShareService shareService)
        {
            this.shareService = shareService;
        }

        protected int ConnectedUserId
            => int.TryParse((User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.PrimarySid)?.Value, out int id) ? id : 0;

        [HttpGet]
        [Authorize]
        public dynamic Get()
        {
            return shareService.GetSharesForUser(ConnectedUserId);
        }
    }
}