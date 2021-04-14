using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Метод GetAll");
            IList<RamMetrics> metrics = _repository.GetAll();
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
