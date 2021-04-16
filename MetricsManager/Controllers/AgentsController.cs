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
        public AgentsController()
        {

        }
        /// <summary>
        /// Зарегистрировать агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///    {
        ///
        /// "AgentAddress" : https://localhost:5001
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response>  
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                connection.Execute(@"INSERT INTO agent(AgentAddress) VALUES(@AgentAddress)",
                new
                {
                    AgentAddress = agentInfo.AgentAddress.ToString()
                });
            }
            return Ok();
        }
        /// <summary>
        /// Получает список агентов
        /// </summary>
        /// <returns>список агентов</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response>  
        [HttpGet("сatalogRegisterAgent")]
        public IList<AgentInfo> СatalogRegisterAgent()
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<AgentInfo>("SELECT AgentId, AgentAddress FROM agent").ToList();
            }
        }
    }
   
}
