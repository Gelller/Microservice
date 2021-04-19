using Dapper;
using MetricsManager.DAL.Repository;
using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    public class LastDate
    {
        public LastDate()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }      
        public DateTimeOffset GetTimeFromTable(string table, int agentid)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<DateTimeOffset>($"SELECT MAX (Time) From {table} WHERE AgentId={agentid}").Single();        
            }
        }
    }
}
