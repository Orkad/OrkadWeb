using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrkadWeb.Angular.Controllers
{
    /// <summary>
    /// Classe de base dont tous les controlleurs de l'api doivent hériter
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public abstract class ApiController : ControllerBase
    {
        
    }
}
