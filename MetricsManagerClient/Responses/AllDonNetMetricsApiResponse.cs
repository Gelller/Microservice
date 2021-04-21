using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManagerClient.Responses
{
    public class AllDotNetMetricsApiResponse
    {
        public List<DotNetMetricsDto> Metrics { get; set; }
    }
    public class DotNetMetricsDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
