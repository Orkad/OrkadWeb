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
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IDataService dataService;

        public TestController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public void Get()
        {
            var test = dataService.Query<User>().ToList();
        }
    }
}