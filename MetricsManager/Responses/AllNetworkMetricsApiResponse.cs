using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses
{
    public class AllNetworkMetricsApiResponse
    {
        public List<NetworkMetricsDto> Metrics { get; set; }
    }
    public class NetworkMetricsDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
