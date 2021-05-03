using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManagerClient.Responses
{
    public class AllHddMetricsApiResponse
    {
        public List<HddMetricsDto> Metrics { get; set; }
    }
    public class HddMetricsDto
    {
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
