using MetricsManagerClient.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MetricsManagerClient
{
   
    class MetricsFromManager
    {
        private HttpClient _httpClient=new HttpClient();
        public AllCpuMetricsApiResponse MetricsFromTable(string table)
        {
            var fromTime = DateTimeOffset.UtcNow.AddMinutes(-1).ToString("O"); ;
            var toTime = DateTimeOffset.UtcNow.ToString("O");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5005/api/metrics/{table}/from/{fromTime}/to/{toTime}");
              try
              {
            
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<AllCpuMetricsApiResponse>(content);

              }
            catch (Exception e)
              {
                return null;
              }
        }
    }
}
