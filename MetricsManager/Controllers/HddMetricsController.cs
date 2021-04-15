﻿using Microsoft.AspNetCore.Mvc;
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
        private IHddMetricsRepository _repository;
        private IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IMapper _mapper;

        public HddMetricsController(IHddMetricsRepository repository, IMetricsAgentClient metricsAgentClient, ILogger<HddMetricsController> logger, IMapper mapper)
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
            IList<HddMetrics> metrics = _repository.GetAll();
            var response = new AllHddMetricsApiResponse()
            {
                Metrics = new List<HddMetricsDto>()
            };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<HddMetricsDto>(metric));
                }
            }
            return Ok(response);
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {          
             _logger.LogInformation($"starting new request to metrics agent");        
            var metrics = _metricsAgentClient.GetAllHddMetrics(new GetAllHddMetricsApiRequest
            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            if (metrics != null)
            {
                foreach (var item in metrics.Metrics)
                    _repository.Create(new HddMetrics
                    {
                        AgentId = agentId,
                        Time = item.Time,
                        Value = item.Value
                    }); ;
            }
            return Ok(metrics);
        }
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
          [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"Метод GetMetricsByPercentileFromAgent fromTime {fromTime} toTime {toTime}");
            var metrics = _metricsAgentClient.GetAllHddMetrics(new GetAllHddMetricsApiRequest
            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            if (metrics != null)
            {
                double[] masValue = new double[metrics.Metrics.Count];
                for (int i = 0; i < masValue.Length; i++)
                {
                    masValue[i] = metrics.Metrics[i].Value;
                }

                var percentileCalculationMethod = new PercentileCalculationMethod();
                var percentileValue = percentileCalculationMethod.PercentileCalculation(masValue, (double)percentile / 100);
                return Ok(percentileValue);
            }
            return Ok();
        }

    }
}
