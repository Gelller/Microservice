using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class HddMetricsUnitTests
    {
        private HddMetricsController controller;
        public HddMetricsUnitTests()
        {
            controller = new HddMetricsController();
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var result = controller.GetMetricsFromAgent();
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
