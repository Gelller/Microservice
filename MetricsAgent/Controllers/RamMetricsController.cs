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

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : Controller
    {
        private readonly ILogger<RamMetricsController> _logger;
        private IRamMetricsRepository _repository;
        public RamMetricsController(IRamMetricsRepository repository, ILogger<RamMetricsController> logger)
        {
            _logger = logger;
            this._repository = repository;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricsCreateRequest request)
        {
            _logger.LogInformation($"Метод Create {request}");
            _repository.Create(new RamMetrics
            {
                Value = request.Value
            });

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Метод GetAll");
            var metrics = _repository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricsDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricsDto { Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }
        [HttpGet("available")]
        public IActionResult GetMetricsFromAgent()
        {
            _logger.LogInformation($"Метод GetMetricsFromAgent");
            return Ok();
        }
    }
}
