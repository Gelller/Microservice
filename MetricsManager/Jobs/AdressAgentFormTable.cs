using Dapper;
using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    public class AdressAgentFormTable
    {
        public List<AgentInfo> GetAllAdress()
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<AgentInfo>($"SELECT AgentAddress, agentid FROM agent").ToList();
            }
        }
    }
}
