using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using MetricsManager.DAL.Interfaces;
using AutoMapper;
using MetricsManager.Client;
using AutoFixture;
using MetricsManager.Models;
using System.Collections.Generic;
using MetricsManager.Responses;
using MetricsManager.Requests;

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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RamMetrics, RamMetricsDto>());
            var mapper = config.CreateMapper();
            _metricsAgentClient = new Mock<IMetricsAgentClient>();
            _logger = new Mock<ILogger<RamMetricsController>>();
            _controller = new RamMetricsController(_mock.Object, _metricsAgentClient.Object, mapper, _logger.Object);
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var fixture = new Fixture();
            var returnList = fixture.Create<List<RamMetrics>>();

            _mock.Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(returnList);

            var fromTime = DateTimeOffset.Now.AddHours(-1);
            var toTime = DateTimeOffset.Now;
            var result = (OkObjectResult)_controller.GetMetricsFromAgent(fromTime, toTime);

            var actualResult = (AllRamMetricsApiResponse)result.Value;
            _mock.Verify(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.Equal(returnList[0].Id, actualResult.Metrics[0].Id);
        }
        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {

            int agentId = 1;
            Percentile percentile;
            percentile = Percentile.P75;
            var fixture = new Fixture();
            var returnList = fixture.Create<AllRamMetricsApiResponse>();

            double[] masValue = new double[returnList.Metrics.Count];
            for (int i = 0; i < masValue.Length; i++)
            {
                masValue[i] = returnList.Metrics[i].Value;
            }
            var percentileCalculationMethod = new PercentileCalculationMethod();
            var percentileValue = percentileCalculationMethod.PercentileCalculation(masValue, (double)percentile / 100);

            _metricsAgentClient.Setup(repository => repository.GetAllRamMetrics(It.IsAny<GetAllRamMetricsApiRequest>())).Returns(returnList);

            var fromTime = DateTimeOffset.Now.AddHours(-1);
            var toTime = DateTimeOffset.Now;
            var result = (OkObjectResult)_controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);

            var actualResult = result.Value;
            _metricsAgentClient.Verify(repository => repository.GetAllRamMetrics(It.IsAny<GetAllRamMetricsApiRequest>()));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.Equal(actualResult, percentileValue);
        }
    }
}
