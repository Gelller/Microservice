using Dapper;
using MetricsManager.Client;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repository;
using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MetricsManager.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;
        private IMetricsAgentClient _metricsAgent;
        public NetworkMetricJob(INetworkMetricsRepository repository, IMetricsAgentClient metricsAgent)
        {
            _repository = repository;
            _metricsAgent = metricsAgent;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var adress = new AdressAgentFormTable();
            List<AgentInfo> uriAdress = adress.GetAllAdress();
            foreach (var adressAgent in uriAdress)
            {
                var fromTimeTable = new LastDate();
                var fromTimeFromTable = fromTimeTable.GetTimeFromTable("networkmetrics", adressAgent.AgentId);
                AllNetworkMetricsApiResponse networkMetrics = null;

                if (fromTimeFromTable == null)
                {
                    DateTimeOffset fromTime = DateTimeOffset.UnixEpoch;
                    DateTimeOffset toTime = DateTimeOffset.UtcNow;
                    networkMetrics = _metricsAgent.GetAllNetworkMetrics(new Requests.GetAllNetworkMetricsApiRequest { ClientBaseAddress = new Uri(adressAgent.AgentAddress), ToTime = toTime, FromTime = fromTime });
                }
                else
                {
                    DateTimeOffset toTime = DateTimeOffset.UtcNow;
                    networkMetrics = _metricsAgent.GetAllNetworkMetrics(new Requests.GetAllNetworkMetricsApiRequest { ClientBaseAddress = new Uri(adressAgent.AgentAddress), ToTime = toTime, FromTime = fromTimeFromTable });
                }
                if (networkMetrics != null)
                    foreach (var item in networkMetrics.Metrics)
                    _repository.Create(new NetworkMetrics
                    {
                        AgentId = adressAgent.AgentId,
                        Time = item.Time,
                        Value = item.Value
                    });
            }
            return Task.CompletedTask;
        }
    }
}
