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
    public class HddMetricsRepository : IHddMetricsRepository
    {
        // строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public HddMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(HddMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров     
                connection.Execute(@"INSERT INTO hddmetrics(value, time)  VALUES(@value, @time)",
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
        public IList<HddMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<HddMetrics>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            }
        }
        
        public IList<HddMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<HddMetrics>($"SELECT Id, Time, Value FROM hddmetrics WHERE Time>={fromTime.ToUnixTimeSeconds()} AND Time<={toTime.ToUnixTimeSeconds()}").ToList();
            }
        }
    }
}
