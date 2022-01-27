using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
    }
}
