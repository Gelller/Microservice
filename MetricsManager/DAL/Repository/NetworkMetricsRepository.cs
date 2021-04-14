using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsManager.DAL.Interfaces;
using Dapper;
using System.Linq;
using System.IO;

namespace MetricsManager.DAL.Repository
{
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        // строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public NetworkMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(NetworkMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"INSERT INTO networkmetrics(value, time, agentid ) VALUES(@value, @time, @agentid )",
              new
              {
                  agentid = item.AgentId,
                  value = item.Value,
                  time = item.Time.ToUnixTimeSeconds()
              });
            }
        }
        public IList<NetworkMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetworkMetrics>("SELECT Id, Time, Value, AgentId FROM networkmetrics").ToList();
            }
        }
        public IList<NetworkMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetworkMetrics>($"SELECT Id, Time, Value FROM networkmetrics WHERE Time>{fromTime.ToUnixTimeSeconds()} AND Time<{toTime.ToUnixTimeSeconds()}").ToList();
            }
        }
    }
}
