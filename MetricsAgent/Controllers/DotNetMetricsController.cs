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
    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : Controller
    {
        private IDotNetMetricsRepository _repository;
        public DotNetMetricsController(IDotNetMetricsRepository repository)
        {
            this._repository = repository;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricsCreateRequest request)
        {
            _repository.Create(new DotNetMetrics
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll();

            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricsDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricsDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {        
            return Ok();
        }
    }
}
