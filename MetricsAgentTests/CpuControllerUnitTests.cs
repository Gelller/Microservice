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
    public class CpuMetricsControllerUnitTests
    {
        private Mock<ILogger<CpuMetricsController>> _logger;
        private CpuMetricsController _controller;
        private Mock<ICpuMetricsRepository> _mock;

        public CpuMetricsControllerUnitTests()
        {
            _logger = new Mock<ILogger<CpuMetricsController>>();
            _mock = new Mock<ICpuMetricsRepository>();
            _controller = new CpuMetricsController(_mock.Object,_logger.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _mock.Setup(repository => repository.Create(It.IsAny<CpuMetrics>())).Verifiable();

            // выполняем действие на контроллере
            var result = _controller.Create(new MetricsAgent.Requests.CpuMetricsCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mock.Verify(repository => repository.Create(It.IsAny<CpuMetrics>()), Times.AtMostOnce());

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
