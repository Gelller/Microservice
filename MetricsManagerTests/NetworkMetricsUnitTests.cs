﻿using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class NetworkMetricsUnitTests
    {
        private NetworkMetricsController _controller;
        public NetworkMetricsUnitTests()
        {
            _controller = new NetworkMetricsController();
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
