using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    public class UriAdress
    {
        public Uri GetUri(int agentId)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return new Uri(connection.Query<string>($"SELECT AgentAddress FROM agent WHERE AgentId={agentId}").Single());
            }
        }
    }

}
