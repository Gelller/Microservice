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
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;
        private IMetricsAgentClient _metricsAgent;
        public HddMetricJob(IHddMetricsRepository repository, IMetricsAgentClient metricsAgent)
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
                var fromTimeFromTable = fromTimeTable.GetTimeFromTable("hddmetrics", adressAgent.AgentId);
                AllHddMetricsApiResponse hddMetrics = null;

                if (fromTimeFromTable == null)
                {
                    DateTimeOffset fromTime = DateTimeOffset.UnixEpoch;
                    DateTimeOffset toTime = DateTimeOffset.UtcNow;
                    hddMetrics = _metricsAgent.GetAllHddMetrics(new Requests.GetAllHddMetricsApiRequest { ClientBaseAddress = new Uri(adressAgent.AgentAddress), ToTime = toTime, FromTime = fromTime });
                }
                else
                {
                    DateTimeOffset toTime = DateTimeOffset.UtcNow;
                    hddMetrics = _metricsAgent.GetAllHddMetrics(new Requests.GetAllHddMetricsApiRequest { ClientBaseAddress = new Uri(adressAgent.AgentAddress), ToTime = toTime, FromTime = fromTimeFromTable });
                }
                foreach (var item in hddMetrics.Metrics)
                    _repository.Create(new HddMetrics
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
