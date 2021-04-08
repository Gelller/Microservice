using MetricsAgent.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private IRamMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _ramCounter;
        public RamMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IRamMetricsRepository>();
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }
        public Task Execute(IJobExecutionContext context)
        {
            var ramUsageInPercents = Convert.ToInt32(_ramCounter.NextValue());
            _repository.Create(new Models.RamMetrics { Value = ramUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
