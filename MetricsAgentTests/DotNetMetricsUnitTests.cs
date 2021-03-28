using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Moq;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;


namespace MetricsAgentTests
{
    public class DotNetMetricsUnitTests
    {
        private DotNetMetricsController _controller;
        private Mock<IDotNetMetricsRepository> _mock;
        public DotNetMetricsUnitTests()
        {
            _mock = new Mock<IDotNetMetricsRepository>();
            _controller = new DotNetMetricsController(_mock.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _mock.Setup(repository => repository.Create(It.IsAny<DotNetMetrics>())).Verifiable();
            // выполняем действие на контроллере
            var result = _controller.Create(new MetricsAgent.Requests.DotNetMetricsCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mock.Verify(repository => repository.Create(It.IsAny<DotNetMetrics>()), Times.AtMostOnce());
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
