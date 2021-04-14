﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsManager.Responses;
using MetricsManager.Client;
using MetricsManager.Requests;
using AutoMapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Models;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository _repository;
        private readonly ILogger<CpuMetricsController> _logger;
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        public CpuMetricsController(ICpuMetricsRepository repository, IMapper mapper, ILogger<CpuMetricsController> logger, IMetricsAgentClient metricsAgentClient)
        {
            _repository= repository;
            _mapper = mapper;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"starting new request to metrics agent");    
            var metrics = _metricsAgentClient.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest

            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            foreach(var item in metrics.Metrics)
            _repository.Create(new CpuMetrics
            {
                AgentId=agentId,
                Time = item.Time,
                Value = item.Value
            });;
            return Ok(metrics);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"Метод GetMetricsByPercentileFromAgent fromTime {fromTime} toTime {toTime}");
            var metrics = _metricsAgentClient.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest

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

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Метод GetAll");
            IList<CpuMetrics> metrics = _repository.GetAll();
            var response = new AllCpuMetricsApiResponse()
            {
                Metrics = new List<CpuMetricsDto>()
            };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<CpuMetricsDto>(metric));
                }
            }
            return Ok(response);
        }
    }
}
