using MetricsAgent.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private IDotNetMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _dotnetCounter;
        public DotNetMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IDotNetMetricsRepository>();
            _dotnetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps","_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
             var dotnetUsageInPercents = Convert.ToInt32(_dotnetCounter.NextValue());
             DateTimeOffset time = DateTimeOffset.UtcNow;
            _repository.Create(new Models.DotNetMetrics { Time = time, Value = dotnetUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
