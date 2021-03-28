using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Requests;
using MetricsAgent.Responses;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : Controller
    {
        private IHddMetricsRepository _repository;
        public HddMetricsController(IHddMetricsRepository repository)
        {
            this._repository = repository;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricsCreateRequest request)
        {
            _repository.Create(new HddMetrics
            {
                Value = request.Value
            });

            return Ok();
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
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
        [HttpGet()]
        public IActionResult GetMetricsFromAgent()
        {
            return Ok();
        }
    }
}
