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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : Controller
    {
        private readonly ILogger<HddMetricsController> _logger;
        private IHddMetricsRepository _repository;
        public HddMetricsController(IHddMetricsRepository repository, ILogger<HddMetricsController> logger)
        {
            _logger = logger;
            this._repository = repository;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricsCreateRequest request)
        {
            _logger.LogInformation($"Метод Create {request}");
            _repository.Create(new HddMetrics
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
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricsDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricsDto { Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }
        [HttpGet("left")]
        public IActionResult GetMetricsFromAgent()
        {
            _logger.LogInformation($"Метод GetMetricsFromAgent agentId");
            return Ok();
        }
    }
}
