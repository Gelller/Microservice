﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsManager.Client;
using AutoMapper;
using MetricsManager.Requests;
using MetricsManager.Models;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Responses;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : Controller
    {
        private IRamMetricsRepository _repository;
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        private readonly ILogger<RamMetricsController> _logger;
        public RamMetricsController(IRamMetricsRepository repository, IMetricsAgentClient metricsAgentClient, IMapper mapper, ILogger<RamMetricsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Метод GetAll");
            IList<RamMetrics> metrics = _repository.GetAll();
            var response = new AllRamMetricsApiResponse()
            {
                Metrics = new List<RamMetricsDto>()
            };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<RamMetricsDto>(metric));
                }
            }
            return Ok(response);
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"starting new request to metrics agent");         
            var metrics = _metricsAgentClient.GetAllRamMetrics(new GetAllRamMetricsApiRequest
            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            foreach (var item in metrics.Metrics)
                _repository.Create(new RamMetrics
                {
                    AgentId = agentId,
                    Time = item.Time,
                    Value = item.Value
                }); ;
            return Ok(metrics);
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
       [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"Метод GetMetricsByPercentileFromAgent fromTime {fromTime} toTime {toTime}");
            var metrics = _metricsAgentClient.GetAllRamMetrics(new GetAllRamMetricsApiRequest
            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            double[] masValue = new double[metrics.Metrics.Count];
            for (int i = 0; i < masValue.Length; i++)
            {
                masValue[i] = metrics.Metrics[i].Value;
            }

            var percentileCalculationMethod = new PercentileCalculationMethod();
            var percentileValue = percentileCalculationMethod.PercentileCalculation(masValue, (double)percentile / 100);
            return Ok(percentileValue);
        }
    }
}
