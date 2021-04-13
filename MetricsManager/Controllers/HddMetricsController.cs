using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsManager.Client;
using MetricsManager.Requests;
using MetricsManager.Responses;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.DAL.Interfaces;
using AutoMapper;
using MetricsManager.Models;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : Controller
    {
        private IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IMapper _mapper;

        public HddMetricsController(IMetricsAgentClient metricsAgentClient, ILogger<HddMetricsController> logger, IMapper mapper)
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
            var metrics = _metricsAgentClient.GetAllHddMetrics(new GetAllHddMetricsApiRequest

            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(metrics);
        }
      
    }
}
