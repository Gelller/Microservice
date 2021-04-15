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
    public class DotNetMetricsControllerUnitTests
    {
        private Mock<IMetricsAgentClient> _metricsAgentClient;
        private DotNetMetricsController _controller;
        private Mock<IDotNetMetricsRepository> _mock;
        private Mock<IMapper> _imapper;
        private Mock<ILogger<DotNetMetricsController>> _logger;

        public DotNetMetricsControllerUnitTests()
        {
            _mock = new Mock<IDotNetMetricsRepository>();
            _imapper = new Mock<IMapper>();
            _metricsAgentClient = new Mock<IMetricsAgentClient>();
            _logger = new Mock<ILogger<DotNetMetricsController>>();
            _controller = new DotNetMetricsController(_mock.Object, _metricsAgentClient.Object,_imapper.Object,_logger.Object);
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
