using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Moq;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL.Interfaces;
using AutoMapper;


namespace MetricsAgentTests
{
    public class HddMetricsUnitTests
    {
        private Mock<ILogger<HddMetricsController>> _logger;
        private HddMetricsController _controller;
        private Mock<IHddMetricsRepository> _mock;
        private Mock<IMapper> _imapper;
        public HddMetricsUnitTests()
        {
            _logger = new Mock<ILogger<HddMetricsController>>();
            _mock = new Mock<IHddMetricsRepository>();
            _imapper = new Mock<IMapper>();
            _controller = new HddMetricsController(_mock.Object,_logger.Object, _imapper.Object);
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
        public void GetAll_ReturnsOk()
        {
            var result = _controller.GetAll();
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }     
    }
}
