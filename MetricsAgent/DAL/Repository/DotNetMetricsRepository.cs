using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.DAL.Interfaces;
using Dapper;
using System.Linq;
using System.Data;
using System.IO;

namespace MetricsAgent.DAL.Repository
{
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        public DotNetMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(DotNetMetrics item)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {   
                connection.Execute(@"INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)",
              new
              {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
              });
            }
        }
        public IList<DotNetMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<DotNetMetrics>("SELECT Id, Time, Value FROM dotnetmetrics").ToList();
            }

        }     
        public IList<DotNetMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<DotNetMetrics>($"SELECT Id, Time, Value FROM dotnetmetrics WHERE Time>={fromTime.ToUnixTimeSeconds()} AND Time<={toTime.ToUnixTimeSeconds()}").ToList();
            }
        }           
    }
}
