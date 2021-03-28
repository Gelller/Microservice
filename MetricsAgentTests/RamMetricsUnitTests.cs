using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class RamMetricsUnitTests
    {
        private RamMetricsController _controller;
        public RamMetricsUnitTests()
        {
            _controller = new RamMetricsController();
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var result = _controller.GetMetricsFromAgent();
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
