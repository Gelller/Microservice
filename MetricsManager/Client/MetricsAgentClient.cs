using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.Controllers;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data.SQLite;
using Dapper;
using System.Collections.Generic;
using MetricsManager.Models;
using System.Linq;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private ICpuMetricsRepository _repositoryCpu;
        private IDotNetMetricsRepository _repositoryDotNet;
        private IRamMetricsRepository _repositoryRam;
        private IHddMetricsRepository _repositoryHdd;
        private INetworkMetricsRepository _repositoryNetwork;
        private HttpClient _httpClient;
        private readonly ILogger<HddMetricsController> _loggerHddMetrics;
        private readonly ILogger<CpuMetricsController> _loggerCpuMetrics;
  
        public MetricsAgentClient(INetworkMetricsRepository repositoryNetwork, IHddMetricsRepository repositoryHdd, IRamMetricsRepository repositoryRam, IDotNetMetricsRepository repositoryDotNet, ICpuMetricsRepository repositoryCpu, ILogger<CpuMetricsController> loggerCpuMetrics, HttpClient httpClient, ILogger<HddMetricsController> logger)
        {
            _repositoryNetwork = repositoryNetwork;
            _repositoryHdd = repositoryHdd;
            _repositoryRam = repositoryRam;
            _repositoryDotNet = repositoryDotNet;
            _repositoryCpu = repositoryCpu;
            _loggerCpuMetrics = loggerCpuMetrics;
            _httpClient = httpClient;
            _loggerHddMetrics = logger;
        }

        private List<AgentInfo> GetUri(int number)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<AgentInfo>($"SELECT AgentAddress FROM agent WHERE AgentId={number}").ToList();
            }
        }
        public AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);
           var uri = new Uri(uriAdress[0].AgentAddress);
           var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/hdd/from/{fromParameter}/to/{toParameter}");
            try
            {
                    HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    using var streamReader = new StreamReader(responseStream);
                    var content = streamReader.ReadToEnd();

                     foreach (var item in JsonConvert.DeserializeObject<AllHddMetricsApiResponse>(content).Metrics)
                    _repositoryHdd.Create(new HddMetrics
                    {
                        AgentId = request.ClientBaseAddress,
                        Time = item.Time,
                        Value = item.Value
                    });
                return JsonConvert.DeserializeObject<AllHddMetricsApiResponse>(content);
            }
            catch (Exception ex)
            {
                    _loggerHddMetrics.LogError(ex.Message);
                    return null;
            }
            
        }
        public AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);
            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/ram/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                foreach (var item in JsonConvert.DeserializeObject<AllRamMetricsApiResponse>(content).Metrics)
                    _repositoryRam.Create(new RamMetrics
                    {
                        AgentId = request.ClientBaseAddress,
                        Time = item.Time,
                        Value = item.Value
                    });
                return JsonConvert.DeserializeObject<AllRamMetricsApiResponse>(content);
            }
            catch (Exception ex)
            {
                _loggerCpuMetrics.LogError(ex.Message);
                return null;
            }
        }
        public AllCpuMetricsApiResponse GetAllCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O"); 
            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);
            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/cpu/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                foreach (var item in JsonConvert.DeserializeObject<AllCpuMetricsApiResponse>(content).Metrics)
                    _repositoryCpu.Create(new CpuMetrics
                    {
                        AgentId = request.ClientBaseAddress,
                        Time = item.Time,
                        Value = item.Value
                    });
                return JsonConvert.DeserializeObject<AllCpuMetricsApiResponse>(content);
            }
            catch (Exception ex)
            {
                _loggerCpuMetrics.LogError(ex.Message);
                return null;
            }
        }  
        public AllDotNetMetricsApiResponse GetAllDotNetMetrics(GetAllDotNetMetrisApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);
            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/dotnet/errors-count/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                foreach (var item in JsonConvert.DeserializeObject<AllDotNetMetricsApiResponse>(content).Metrics)
                    _repositoryDotNet.Create(new DotNetMetrics
                    {
                        AgentId = request.ClientBaseAddress,
                        Time = item.Time,
                        Value = item.Value
                    });
                return JsonConvert.DeserializeObject<AllDotNetMetricsApiResponse>(content);
            }
            catch (Exception ex)
            {
                _loggerHddMetrics.LogError(ex.Message);
                return null;
            }
        }

        public AllNetworkMetricsApiResponse GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);
            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/network/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                foreach (var item in JsonConvert.DeserializeObject<AllNetworkMetricsApiResponse>(content).Metrics)
                    _repositoryNetwork.Create(new NetworkMetrics
                    {
                        AgentId = request.ClientBaseAddress,
                        Time = item.Time,
                        Value = item.Value
                    });
                return JsonConvert.DeserializeObject<AllNetworkMetricsApiResponse>(content);
            }
            catch (Exception ex)
            {
                _loggerHddMetrics.LogError(ex.Message);
                return null;
            }
        }
    }
}
