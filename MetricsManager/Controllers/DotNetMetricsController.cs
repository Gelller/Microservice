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
    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : Controller
    {
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        private readonly ILogger<DotNetMetricsController> _logger;
        public DotNetMetricsController(IMetricsAgentClient metricsAgentClient, IMapper mapper, ILogger<DotNetMetricsController> logger)
        {
            _mapper = mapper;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис          
            var metrics = _metricsAgentClient.GetAllDotNetMetrics(new GetAllDotNetMetrisApiRequest

            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(metrics);
        }

    }
}
