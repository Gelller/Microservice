using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace MetricsManagerTests
{
    public class NetworkMetricsUnitTests
    {
        private Mock<ILogger<NetworkMetricsController>> _logger;
        private NetworkMetricsController _controller;
        public NetworkMetricsUnitTests()
        {
            _logger = new Mock<ILogger<NetworkMetricsController>>();
            _controller = new NetworkMetricsController(_logger.Object);
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
