using Dapper;
using MetricsManager.Client;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repository;
using MetricsManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;


namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private IMetricsAgentClient _repository;
        public CpuMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IMetricsAgentClient>();;
        }
        public Task Execute(IJobExecutionContext context)
        {
                var fromTimeTable = new GetTime();
                var fromTimeFromtheTable = fromTimeTable.GetTimeCpu("cpumetrics");
            if (fromTimeFromtheTable == null)
            {
                DateTimeOffset fromTime = DateTimeOffset.UtcNow.AddHours(-1);
                DateTimeOffset toTime = DateTimeOffset.UtcNow;
                int agentId = 1;
                _repository.GetAllCpuMetrics(new Requests.GetAllCpuMetricsApiRequest { ClientBaseAddress = agentId, ToTime = toTime, FromTime = fromTime });
            }
            else
            {
                DateTimeOffset toTime = DateTimeOffset.UtcNow;
                int agentId = 1;
                _repository.GetAllCpuMetrics(new Requests.GetAllCpuMetricsApiRequest { ClientBaseAddress = agentId, ToTime = toTime, FromTime = fromTimeFromtheTable.Time });
            }
            return Task.CompletedTask;
        }
    }
}
