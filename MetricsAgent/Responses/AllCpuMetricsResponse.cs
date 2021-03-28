using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricsDto> Metrics { get; set; }
    }
    public class CpuMetricsDto
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }
}
