using MetricsAgent.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private ICpuMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;
        public CpuMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<ICpuMetricsRepository>();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            // узнаем когда мы сняли значение метрики.
            DateTimeOffset time = DateTimeOffset.UtcNow;
            // теперь можно записать что-то при помощи репозитория
            _repository.Create(new Models.CpuMetrics { Time = time, Value = cpuUsageInPercents });
            return Task.CompletedTask;
        }
    }

}
