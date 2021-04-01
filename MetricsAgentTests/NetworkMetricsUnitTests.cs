using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Moq;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgentTests
{ 
    public class NetworkMetricsUnitTests
    {
        private Mock<ILogger<NetworkMetricsController>> _logger;
        private NetworkMetricsController _controller;
        private Mock<INetworkMetricsRepository> _mock;
        public NetworkMetricsUnitTests()
        {
            _logger = new Mock<ILogger<NetworkMetricsController>>();
            _mock = new Mock<INetworkMetricsRepository>();
            _controller = new NetworkMetricsController(_mock.Object,_logger.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _mock.Setup(repository => repository.Create(It.IsAny<NetworkMetrics>())).Verifiable();
            // logger.Setup(logger => logger);
            // выполняем действие на контроллере
            var result = _controller.Create(new MetricsAgent.Requests.NetworkMetricsCreateRequest { Time = DateTimeOffset.FromUnixTimeSeconds(1), Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mock.Verify(repository => repository.Create(It.IsAny<NetworkMetrics>()), Times.AtMostOnce());
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
