using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManagerClient.Responses
{
    public class NumberOfAgentsRegistered
    {
        public List<AgentInfoDto> Values { get; set; }
        public NumberOfAgentsRegistered()
        {
            Values = new List<AgentInfoDto>();
        }
    }
    public class AgentInfoDto
    {
        public int AgentId { get; set; }
        public string AgentAddress { get; set; }
    }
}
