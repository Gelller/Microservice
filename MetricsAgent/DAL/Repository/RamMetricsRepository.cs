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
    public class RamMetricsRepository : IRamMetricsRepository
    {
        // строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public RamMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(RamMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            { 
              connection.Execute(@"INSERT INTO rammetrics(value, time) VALUES(@value, @time)",
              new
              {
                  value = item.Value,
                  time = item.Time.ToUnixTimeSeconds()
              });
            }
        }
        public IList<RamMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<RamMetrics>("SELECT Id, Time, Value FROM rammetrics").ToList();
            }
        }
        public IList<RamMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<RamMetrics>($"SELECT Id, Time, Value FROM rammetrics WHERE Time>={fromTime.ToUnixTimeSeconds()} AND Time<={toTime.ToUnixTimeSeconds()}").ToList();
            }
        }
    }
}
