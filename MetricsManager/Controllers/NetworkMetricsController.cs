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
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : Controller
    {
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        private readonly ILogger<NetworkMetricsController> _logger;
        public NetworkMetricsController(IMetricsAgentClient metricsAgentClient, IMapper mapper, ILogger<NetworkMetricsController> logger)
        {
            _mapper = mapper;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            // логируем, что мы пошли в соседний сервис
            _logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис          
            var metrics = _metricsAgentClient.GetAllNetworkMetrics(new GetAllNetworkMetricsApiRequest

            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(metrics);
        }
    }
}
