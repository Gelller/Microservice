using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsManager.Client;
using AutoMapper;
using MetricsManager.Requests;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : Controller
    {
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        private readonly ILogger<RamMetricsController> _logger;
        public RamMetricsController(IMetricsAgentClient metricsAgentClient, IMapper mapper, ILogger<RamMetricsController> logger)
        {
            _mapper = mapper;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }
        [HttpGet("agent/{agentId}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId)
        {
            _logger.LogInformation($"Метод GetMetricsFromAgent agentId {agentId}");
            return Ok();
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис          
            var metrics = _metricsAgentClient.GetAllRamMetrics(new GetAllRamMetricsApiRequest

            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(metrics);
        }
    }
}
