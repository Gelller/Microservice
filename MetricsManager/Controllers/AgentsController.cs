using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MetricsManager.Responses;
using System.Text.Json;
using MetricsManager.Client;
using MetricsManager.Requests;
using System.Data.SQLite;
using Dapper;
using MetricsManager.Models;
using MetricsManager.DAL.Repository;

namespace MetricsManager.Controllers
{
    [Route("api")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
      //  private AgentInfo _repository;
      //  private NumberOfAgentsRegistered _numberOfAgentsRegistered;
       // private readonly IHttpClientFactory _clientFactory;
      //  private MetricsAgentClient _metricsAgent;

        public AgentsController()
        {

      //      SqlMapper.AddTypeHandler(new StringToUriConverter());
        }

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"INSERT INTO agent(AgentAddress) VALUES(@AgentAddress)",
                new
                {
                    AgentAddress = agentInfo.AgentAddress.ToString()
                });
            }
            return Ok();
        }
        //[HttpPut("enable/{agentId}")]
        //public IActionResult EnableAgentById([FromRoute] int agentId)
        //{
        //    return Ok();
        //}
        //[HttpPut("disable/{agentId}")]
        //public IActionResult DisableAgentById([FromRoute] int agentId)
        //{
        //    return Ok();
        //}

        [HttpGet("сatalogRegisterAgent")]
        public IList<AgentInfo> СatalogRegisterAgent()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
          
                return connection.Query<AgentInfo>("SELECT AgentId, AgentAddress FROM agent").ToList();
            }
        }
    }
   
}
