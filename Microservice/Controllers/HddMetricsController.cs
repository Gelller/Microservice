using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Controllers
{
    [Route("api/metrics/hdd[controller]")]
    [ApiController]
    public class HddMetricsController : Controller
    {
        [HttpGet("agent/{agentId}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId)
        {
             return Ok();
        }
    }
}
