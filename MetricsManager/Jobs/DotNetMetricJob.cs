using Dapper;
using MetricsManager.Client;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Jobs;
using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;
        private IMetricsAgentClient _metricsAgent;
        public DotNetMetricJob(IDotNetMetricsRepository repository, IMetricsAgentClient metricsAgent)
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
                var fromTimeFromTable = fromTimeTable.GetTimeFromTable("dotnetmetrics", adressAgent.AgentId);
                AllDotNetMetricsApiResponse dotNetMetrics = null;

                if (fromTimeFromTable == null)
                {
                    DateTimeOffset fromTime = DateTimeOffset.UnixEpoch;
                    DateTimeOffset toTime = DateTimeOffset.UtcNow;
                    dotNetMetrics = _metricsAgent.GetAllDotNetMetrics(new Requests.GetAllDotNetMetricsApiRequest { ClientBaseAddress = new Uri(adressAgent.AgentAddress), ToTime = toTime, FromTime = fromTime });
                }
                else
                {
                    DateTimeOffset toTime = DateTimeOffset.UtcNow;
                    dotNetMetrics = _metricsAgent.GetAllDotNetMetrics(new Requests.GetAllDotNetMetricsApiRequest { ClientBaseAddress = new Uri(adressAgent.AgentAddress), ToTime = toTime, FromTime = fromTimeFromTable });
                }
                foreach (var item in dotNetMetrics.Metrics)
                    _repository.Create(new DotNetMetrics
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
