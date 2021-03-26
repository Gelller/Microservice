using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
 
    public class RamMetricsUnitTests
    {
        private RamMetricsController controller;
        public RamMetricsUnitTests()
        {
            controller = new RamMetricsController();
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
