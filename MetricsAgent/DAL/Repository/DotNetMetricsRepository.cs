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
        // строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public DotNetMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(DotNetMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров     
                connection.Execute(@"INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)",
              // анонимный объект с параметрами запроса
              new
              {
                    // value подставится на место "@value" в строке запроса
                    // значение запишется из поля Value объекта item
                    value = item.Value,
                    // записываем в поле time количество секунд
                    time = item.Time.ToUnixTimeSeconds()
              });
            }
        }
        public IList<DotNetMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetrics>("SELECT Id, Time, Value FROM dotnetmetrics").ToList();
            }

        }     
        public IList<DotNetMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetrics>($"SELECT Id, Time, Value FROM dotnetmetrics WHERE Time>={fromTime.ToUnixTimeSeconds()} AND Time<={toTime.ToUnixTimeSeconds()}").ToList();
            }
        }           
    }
}
