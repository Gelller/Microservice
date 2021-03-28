using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;


namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController _controller;
        public CpuMetricsControllerUnitTests()
        {
            _controller = new CpuMetricsController();
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var result = _controller.GetMetricsByPercentileFromAgent(fromTime, toTime);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }   
    }
}
