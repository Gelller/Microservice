using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Moq;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgentTests
{
    public class HddMetricsUnitTests
    {
        private HddMetricsController _controller;
        private Mock<IHddMetricsRepository> _mock;
        public HddMetricsUnitTests()
        {
            _mock = new Mock<IHddMetricsRepository>();   
            _controller = new HddMetricsController(_mock.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _mock.Setup(repository => repository.Create(It.IsAny<HddMetrics>())).Verifiable();
            // logger.Setup(logger => logger);
            // выполняем действие на контроллере
            var result = _controller.Create(new MetricsAgent.Requests.HddMetricsCreateRequest { Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mock.Verify(repository => repository.Create(It.IsAny<HddMetrics>()));
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
