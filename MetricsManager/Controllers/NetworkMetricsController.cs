using Microsoft.AspNetCore.Mvc;
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
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : Controller
    {
        private INetworkMetricsRepository _repository;
        private IMetricsAgentClient _metricsAgentClient;
        private readonly IMapper _mapper;
        private readonly ILogger<NetworkMetricsController> _logger;
        public NetworkMetricsController(INetworkMetricsRepository repository, IMetricsAgentClient metricsAgentClient, IMapper mapper, ILogger<NetworkMetricsController> logger)
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
            IList<NetworkMetrics> metrics = _repository.GetAll();
            var response = new AllNetworkMetricsApiResponse()
            {
                Metrics = new List<NetworkMetricsDto>()
            };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<NetworkMetricsDto>(metric));
                }
            }
            return Ok(response);
        }
        /// <summary>
        /// Получает метрики Network на заданном диапазоне времени с агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET agentid/1/from/2000-10-1 01:01:01/to/2100-10-1 01:01:01
        ///
        /// </remarks>
        /// <param name="agentId">id агента</param>
        /// <param name="fromTime">начальная метрка времени</param>
        /// <param name="toTime">конечная метрка времени </param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response> 
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsAgentFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"starting new request to metrics agent");         
            var metrics = _metricsAgentClient.GetAllNetworkMetrics(new GetAllNetworkMetricsApiRequest
            {
                ClientBaseAddress = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            if (metrics != null)
            {
                foreach (var item in metrics.Metrics)
                    _repository.Create(new NetworkMetrics
                    {
                        AgentId = agentId,
                        Time = item.Time,
                        Value = item.Value
                    }); ;
            }
            return Ok(metrics);
        }
        /// <summary>
        /// Получает перцентиль с метрик Network на заданном диапазоне времени с агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET agentid/1/from/2000-10-1 01:01:01/to/2100-10-1 01:01:01/Percentile/P75
        ///
        /// </remarks>
        /// <param name="agentId">id агента</param>
        /// <param name="fromTime">начальная метрка времени</param>
        /// <param name="toTime">конечная метрка времени </param>
        /// <param name="percentile">перцентиль</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response> 
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
        [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"Метод GetMetricsByPercentileFromAgent fromTime {fromTime} toTime {toTime}");
            var metrics = _metricsAgentClient.GetAllNetworkMetrics(new GetAllNetworkMetricsApiRequest
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
        /// <summary>
        /// Получает все метрики Network
        /// </summary>
        /// <returns>Список метрик</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response>  
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Метод GetMetricsFromAgent fromTime {fromTime.DateTime} toTime {toTime.DateTime}");
            var metrics = _repository.GetByTimeInterval(fromTime, toTime);
            var response = new AllNetworkMetricsApiResponse()
            {
                Metrics = new List<NetworkMetricsDto>()
            };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<NetworkMetricsDto>(metric));
                }
            }
            return Ok(response);
        }

    }
}
