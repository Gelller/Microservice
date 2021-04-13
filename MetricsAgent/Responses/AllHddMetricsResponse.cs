using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
    public class AllHddMetricsResponse
    {
        public List<HddMetricsDto> Metrics { get; set; }
    }
    public class HddMetricsDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    
    }
}
