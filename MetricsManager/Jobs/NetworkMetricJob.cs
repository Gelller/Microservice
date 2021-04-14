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

    public class NetworkMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private IMetricsAgentClient _repository;
        public NetworkMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IMetricsAgentClient>(); ;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var fromTimeTable = new GetTime();
            var fromTimeFromtheTable = fromTimeTable.GetTimeNetwork("networkmetrics");
            if (fromTimeFromtheTable == null)
            {
                DateTimeOffset fromTime = DateTimeOffset.UtcNow.LocalDateTime.AddHours(-1);
                DateTimeOffset toTime = DateTimeOffset.UtcNow.LocalDateTime;
                int agentId = 1;
                _repository.GetAllNetworkMetrics(new Requests.GetAllNetworkMetricsApiRequest { ClientBaseAddress = agentId, ToTime = toTime, FromTime = fromTime });
            }
            else
            {
                DateTimeOffset toTime = DateTimeOffset.UtcNow.LocalDateTime;
                int agentId = 1;
                _repository.GetAllNetworkMetrics(new Requests.GetAllNetworkMetricsApiRequest { ClientBaseAddress = agentId, ToTime = toTime, FromTime = fromTimeFromtheTable.Time });
            }
            return Task.CompletedTask;
        }
    }
}
