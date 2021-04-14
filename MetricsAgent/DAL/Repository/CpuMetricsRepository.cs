using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.DAL.Interfaces;
using Dapper;
using System.Linq;
using System.IO;
using MetricsAgent.Controllers;

namespace MetricsAgent.DAL.Repository
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        // строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public CpuMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(CpuMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров     
                  connection.Execute(@"INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
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
        public IList<CpuMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetrics>("SELECT Id, Time, Value FROM cpumetrics").ToList();
            }
          
        }
        public IList<CpuMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetrics>($"SELECT Id, Time, Value FROM cpumetrics WHERE Time>{fromTime.ToUnixTimeSeconds()} AND Time<{toTime.ToUnixTimeSeconds()}").ToList();
            }      
        }
       
    }
}