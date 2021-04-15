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
    public class CpuMetricsControllerUnitTests
    {
        private Mock<IMetricsAgentClient> _metricsAgentClient;
        private CpuMetricsController _controller;
        private Mock<ICpuMetricsRepository> _mock;
        private Mock<ILogger<CpuMetricsController>> _logger;

        public CpuMetricsControllerUnitTests()
        {
            _mock = new Mock<ICpuMetricsRepository>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CpuMetrics, CpuMetricsDto>());
            var mapper = config.CreateMapper();
            _metricsAgentClient = new Mock<IMetricsAgentClient>();
            _logger = new Mock<ILogger<CpuMetricsController>>();
            _controller = new CpuMetricsController(_mock.Object, mapper, _logger.Object, _metricsAgentClient.Object);
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var fixture = new Fixture();
            var returnList = fixture.Create<List<CpuMetrics>>();

            _mock.Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(returnList);

            var fromTime = DateTimeOffset.Now.AddHours(-1);
            var toTime = DateTimeOffset.Now;
            var result = (OkObjectResult)_controller.GetMetricsFromAgent(fromTime, toTime);

            var actualResult = (AllCpuMetricsApiResponse)result.Value;
            _mock.Verify(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.Equal(returnList[0].Id,actualResult.Metrics[0].Id);
        }
        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {

            int agentId = 1;
            Percentile percentile;
            percentile = Percentile.P75;         
            var fixture = new Fixture();
            var returnList = fixture.Create<AllCpuMetricsApiResponse>();

            double[] masValue = new double[returnList.Metrics.Count];
            for (int i = 0; i < masValue.Length; i++)
            {
                masValue[i] = returnList.Metrics[i].Value;
            }
            var percentileCalculationMethod = new PercentileCalculationMethod();
            var percentileValue = percentileCalculationMethod.PercentileCalculation(masValue, (double)percentile / 100);

            _metricsAgentClient.Setup(repository => repository.GetAllCpuMetrics(It.IsAny<GetAllCpuMetricsApiRequest>())).Returns(returnList);

            var fromTime = DateTimeOffset.Now.AddHours(-1);
            var toTime = DateTimeOffset.Now;
            var result = (OkObjectResult)_controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);

            var actualResult =result.Value;       
            _metricsAgentClient.Verify(repository => repository.GetAllCpuMetrics(It.IsAny<GetAllCpuMetricsApiRequest>()));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.Equal(actualResult, percentileValue);
        }
    }   
}
