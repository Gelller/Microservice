using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.DAL.Interfaces;
using Dapper;
using System.Linq;
using System.IO;

namespace MetricsAgent.DAL.Repository
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        public CpuMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(CpuMetrics item)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {  
                  connection.Execute(@"INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                new
                {
                         value = item.Value,
                         time = item.Time.ToUnixTimeSeconds()
                     });
            }
        } 
        public IList<CpuMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<CpuMetrics>("SELECT Id, Time, Value FROM cpumetrics").ToList();
            }      
        }     
        public IList<CpuMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<CpuMetrics>($"SELECT Id, Time, Value FROM cpumetrics WHERE Time>={fromTime.ToUnixTimeSeconds()} AND Time<={toTime.ToUnixTimeSeconds()}").ToList();
            }      
        }
    }
}