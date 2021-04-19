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
        private HttpClient _httpClient;
        private readonly ILogger<CpuMetricsController> _loggerCpu;
        private readonly ILogger<HddMetricsController> _loggerHdd;
        private readonly ILogger<DotNetMetricsController> _loggerDotNet;
        private readonly ILogger<NetworkMetricsController> _loggerNetwork;
        private readonly ILogger<RamMetricsController> _loggerRam;

        public MetricsAgentClient(ILogger<CpuMetricsController> loggerCpu, ILogger<HddMetricsController> loggerHdd, ILogger<DotNetMetricsController> loggerDotNet, ILogger<NetworkMetricsController> loggerNetwork, ILogger<RamMetricsController> loggerRam, HttpClient httpClient)
        {
            _loggerCpu = loggerCpu;
            _loggerHdd = loggerHdd;
            _loggerDotNet = loggerDotNet;
            _loggerNetwork = loggerNetwork;
            _loggerRam = loggerRam;
            _httpClient = httpClient;
        }
        public AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
           var fromParameter = request.FromTime.UtcDateTime.ToString("O");
           var toParameter = request.ToTime.UtcDateTime.ToString("O");
           var uri =request.ClientBaseAddress;
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
                    _loggerHdd.LogError(ex.Message);
                    return null;
            }      
        }
        public AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            var uri = request.ClientBaseAddress;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{uri}api/metrics/ram/from/{fromParameter}/to/{toParameter}");
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
                _loggerRam.LogError(ex.Message);
                return null;
            }
        }
        public AllCpuMetricsApiResponse GetAllCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            var uri = request.ClientBaseAddress;
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
                _loggerCpu.LogError(ex.Message);
                return null;
            }
        }  
        public AllDotNetMetricsApiResponse GetAllDotNetMetrics(GetAllDotNetMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            var uri =request.ClientBaseAddress;
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
                _loggerNetwork.LogError(ex.Message);
                return null;
            }
        }

        public AllNetworkMetricsApiResponse GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.UtcDateTime.ToString("O");
            var toParameter = request.ToTime.UtcDateTime.ToString("O");
            var uri = request.ClientBaseAddress;
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
                _loggerDotNet.LogError(ex.Message);
                return null;
            }
        }
    }
}
