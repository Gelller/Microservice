﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL.Interfaces;
using AutoMapper;

namespace MetricsAgent.Controllers
{
   [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : Controller
    {
       // private ICpuMetricsRepository _repository;
        private readonly ILogger<RamMetricsController> _logger;
        private IRamMetricsRepository _repository;
        private readonly IMapper _mapper;
        public RamMetricsController(IRamMetricsRepository repository, ILogger<RamMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricsCreateRequest request)
        {
            _logger.LogInformation($"Метод Create {request}");
            _repository.Create(new RamMetrics
            {
               Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }
        /// <summary>
        /// Получает все метрики Ram
        /// </summary>
        /// <returns>Список метрик</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response>


        [HttpGet("all")]
        public IActionResult GetAll()
        {
           _logger.LogInformation($"Метод GetAll");
            IList<RamMetrics> metrics = _repository.GetAll();
            // IList<RamMetrics> metrics = _repository.GetAll();
            var response = new AllRamMetricsResponse()
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
          //  return Ok();
        }
        /// <summary>
        /// Получает метрики Ram на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /from/2000-10-1 01:01:01/to/2100-10-1 01:01:01
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрка времени</param>
        /// <param name="toTime">конечная метрка времени </param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">если передали не правильные параетры</response> 
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Метод GetMetricsFromAgent fromTime {fromTime.DateTime} toTime {toTime.DateTime}");
            var metrics = _repository.GetByTimeInterval(fromTime, toTime);
            var response = new AllRamMetricsResponse()
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
    }
}
