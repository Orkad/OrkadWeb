using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Services.Business;

namespace OrkadWebVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisionController : ControllerBase
    {
        private readonly SupervisionService supervisionService;

        public SupervisionController(SupervisionService supervisionService)
        {
            this.supervisionService = supervisionService;
        }

        [HttpGet("cpu/temp")]
        public double GetCpuTemperature()
        {
            return supervisionService.GetCpuTemperature();
        }
    }
}