using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Controllers
{
    [Route("api/metrics/ram[controller]")]
    [ApiController]
    public class RamMetricsController : Controller
    {
       [HttpGet("agent/{agentId}")]
       public IActionResult GetMetricsFromAgent([FromRoute] int agentId)
       {
            return Ok();
       }       
    }
}
