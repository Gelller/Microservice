using MetricsAgent.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private INetworkMetricsRepository _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _networkCounter;
        private List<PerformanceCounter> _allnetworkInterface;

        public NetworkMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<INetworkMetricsRepository>();    
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            String[] instancename = category.GetInstanceNames();
            List<string> networkInterface = new List<string>();
            foreach (string name in instancename)
            {
                networkInterface.Add(name);
            }
            _allnetworkInterface = new List<PerformanceCounter>();
            foreach (var item in networkInterface)
            {
                _networkCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", item);        
                _allnetworkInterface.Add(_networkCounter);
            }
        }
        public Task Execute(IJobExecutionContext context)
        {
            int networkUsageInPercents=0;       
            foreach (var item in _allnetworkInterface)
            {
                networkUsageInPercents = networkUsageInPercents+Convert.ToInt32(item.NextValue());

            }
            DateTimeOffset time = DateTimeOffset.UtcNow;
            _repository.Create(new Models.NetworkMetrics { Time = time, Value = networkUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
