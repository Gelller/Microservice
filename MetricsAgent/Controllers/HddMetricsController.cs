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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : Controller
    {
        private readonly ILogger<HddMetricsController> _logger;
        private IHddMetricsRepository _repository;
        private readonly IMapper _mapper;
        public HddMetricsController(IHddMetricsRepository repository, ILogger<HddMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
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
            IList<HddMetrics> metrics = _repository.GetAll();
            var response = new AllHddMetricsResponse()
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
        [HttpGet("left")]
        public IActionResult GetMetricsFromAgent()
        {
            _logger.LogInformation($"Метод GetMetricsFromAgent agentId");
            return Ok();
        }
    }
}
