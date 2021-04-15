using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using MetricsManager.DAL.Interfaces;
using AutoMapper;
using MetricsManager.Client;

namespace MetricsManagerTests
{
    public class RamMetricsControllerUnitTests
    {
        private Mock<IMetricsAgentClient> _metricsAgentClient;
        private RamMetricsController _controller;
        private Mock<IRamMetricsRepository> _mock;
        private Mock<IMapper> _imapper;
        private Mock<ILogger<RamMetricsController>> _logger;

        public RamMetricsControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();
            _imapper = new Mock<IMapper>();
            _metricsAgentClient = new Mock<IMetricsAgentClient>();
            _logger = new Mock<ILogger<RamMetricsController>>();
            _controller = new RamMetricsController(_mock.Object, _metricsAgentClient.Object, _imapper.Object, _logger.Object);
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            int agentId = 1;
            var fromTime = DateTimeOffset.Now.AddHours(-1);
            var toTime = DateTimeOffset.Now;
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {
            int agentId = 1;
            Percentile percentile;
            percentile = Percentile.P75;
            var fromTime = DateTimeOffset.Now.AddHours(-1);
            var toTime = DateTimeOffset.Now;
            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
