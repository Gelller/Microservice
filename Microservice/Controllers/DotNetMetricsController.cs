using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Controllers
{
    [Route("api/metrics/dotnet/errors-count[controller]")]
    [ApiController]
    public class DotNetMetricsController : Controller
    {
       [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
       public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
       {
            return Ok();
       }    
    }
}
