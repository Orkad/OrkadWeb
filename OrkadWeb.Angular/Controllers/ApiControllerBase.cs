using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrkadWeb.Angular.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {

    }
}
