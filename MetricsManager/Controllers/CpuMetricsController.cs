using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsManager.Responses;
using MetricsManager.Client;
using MetricsManager.Requests;
using AutoMapper;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        public CpuMetricsController(IMapper mapper, ILogger<CpuMetricsController> logger, IMetricsAgentClient metricsAgentClient)
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
            var metrics = _metricsAgentClient.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest

            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(metrics);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"Метод GetMetricsByPercentileFromAgent agentId {agentId} fromTime {fromTime} toTime {toTime}");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"Метод GetMetricsFromAllCluster fromTime {fromTime} toTime {toTime}");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"Метод GetMetricsByPercentileFromAllCluster fromTime {fromTime} toTime {toTime}");
            return Ok();
        }           
    }
}
