using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Models;
using OrkadWeb.Services;

namespace OrkadWebVue.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IDataService dataService;

        public TestController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public dynamic Users()
        {
            return dataService.Query<User>()
                .Select(u => new
                {
                    User = u.Username,
                    ShareCount = u.UserShares.Count()
                });
        }

        [HttpGet]
        public dynamic Shares()
        {
            return dataService.Query<Share>()
                .Select(s => new
                {
                    Name = s.Name,
                    UserCount = s.UserShares.Count(),
                });
        }

        [HttpGet]
        public dynamic UserShares()
        {
            return dataService.Query<UserShare>()
                .Select(us => new
                {
                    User = us.User.Username,
                    Share = us.Share.Name
                });
        }

        [HttpGet]
        public dynamic Expenses()
        {
            return dataService.Query<Expense>()
                .Select(e => new
                {
                    User = e.UserShare.User.Username,
                    Amount = e.Amount,
                    Reason = e.Name
                });
        }
    }
}