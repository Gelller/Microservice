using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManagerClient.Responses
{
    public class AllCpuMetricsApiResponse
    {
        public List<CpuMetricsDto> Metrics { get; set; }
    }
    public class CpuMetricsDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
