using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private NumberOfAgentsRegistered _numberOfAgentsRegistered;
        public AgentsController(NumberOfAgentsRegistered registered)
        {
            this._numberOfAgentsRegistered = registered;
        }
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {      
            _numberOfAgentsRegistered.Values.Add(agentInfo);
            return Ok();
        }
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }
        [HttpGet("сatalogRegisterAgent")]
        public IActionResult СatalogRegisterAgent()
        {
            return Ok(_numberOfAgentsRegistered);
        }
    }
    public class NumberOfAgentsRegistered
    {
        public List<AgentInfo> Values { get; set; }
        public NumberOfAgentsRegistered()
        {
            Values = new List<AgentInfo>();
        }
    }
    public class AgentInfo
    {
        public int AgentId { get; set; }
        public Uri AgentAddress { get; set; }
    }
}
