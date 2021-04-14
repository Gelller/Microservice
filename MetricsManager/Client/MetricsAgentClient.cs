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

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private HttpClient _httpClient;
        private readonly ILogger<HddMetricsController> _loggerHddMetrics;
        private readonly ILogger<CpuMetricsController> _loggerCpuMetrics;

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

        public MetricsAgentClient(ILogger<CpuMetricsController> loggerCpuMetrics, HttpClient httpClient, ILogger<HddMetricsController> logger)
        {
            _loggerCpuMetrics = loggerCpuMetrics;
            _httpClient = httpClient;
            _loggerHddMetrics = logger;
        }

        public List<AgentInfo> GetUri(int number)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<AgentInfo>($"SELECT AgentAddress FROM agent WHERE AgentId={number}").ToList();
            }
        }

        public AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.DateTime.ToString("O");
            var toParameter = request.ToTime.DateTime.ToString("O"); 

            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);

           var uri = new Uri(uriAdress[0].AgentAddress);
           var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/hdd/from/{fromParameter}/to/{toParameter}");
            try
            {
                    HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    using var streamReader = new StreamReader(responseStream);
                    var content = streamReader.ReadToEnd();

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
            var fromParameter = request.FromTime.DateTime.ToString("O");
            var toParameter = request.ToTime.DateTime.ToString("O"); 

            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);

            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/cpu2/all");
         //   var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/ram/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();

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
            var fromParameter = request.FromTime.DateTime.ToString("O");
            var toParameter = request.ToTime.DateTime.ToString("O"); 

            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);

            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/cpu/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();

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
            var fromParameter = request.FromTime.DateTime.ToString("O");
            var toParameter = request.ToTime.DateTime.ToString("O"); 

            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);

            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/dotnet/errors-count/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();

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
            var fromParameter = request.FromTime.DateTime.ToString("O");
            var toParameter = request.ToTime.DateTime.ToString("O"); ;

            List<AgentInfo> uriAdress = GetUri(request.ClientBaseAddress);

            var uri = new Uri(uriAdress[0].AgentAddress);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/network/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();

                return JsonConvert.DeserializeObject<AllNetworkMetricsApiResponse>(content);

            }
            catch (Exception ex)
            {
                _loggerHddMetrics.LogError(ex.Message);
                return null;
            }
        }
    }
    // остальные методы реализовать самим
}
