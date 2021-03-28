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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : Controller
    {
        private IRamMetricsRepository _repository;
        public RamMetricsController(IRamMetricsRepository repository)
        {
            this._repository = repository;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricsCreateRequest request)
        {
            _repository.Create(new RamMetrics
            {
                Value = request.Value
            });

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
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
        [HttpGet()]
        public IActionResult GetMetricsFromAgent()
        {   
            return Ok();
        }
    }
}
