using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Services;

namespace OrkadWebVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisionController : ControllerBase
    {
        private readonly ISupervisionService supervisionService;

        public SupervisionController(ISupervisionService supervisionService)
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