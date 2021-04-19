using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Models
{
    public class AgentInfo
    {

        public int AgentId { get; set; }
        public string AgentAddress { get; set; }

    }
}
