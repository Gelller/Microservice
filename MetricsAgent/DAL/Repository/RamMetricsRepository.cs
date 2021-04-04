﻿using MetricsAgent.Models;
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
        // наше соединение с базой данных
        private SQLiteConnection _connection;
        // строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public RamMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void CreateAndOpenDb()
        {
            var dbFilePath = "metrics.db";
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            _connection = new SQLiteConnection(string.Format(
              "Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;", dbFilePath));
            _connection.Execute(@$"CREATE TABLE if not exists rammetrics(id INTEGER PRIMARY KEY, value INT)");
            _connection.Open();
        }
        public void Create(RamMetrics item)
        {
            CreateAndOpenDb();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров     
                connection.Execute(@"INSERT INTO rammetrics(value) VALUES(@value)",
              // анонимный объект с параметрами запроса
              new
              {
                  // value подставится на место "@value" в строке запроса
                  // значение запишется из поля Value объекта item
                  value = item.Value,
                  // записываем в поле time количество секунд
              });
            }
        }
        public IList<RamMetrics> GetAll()
        {
            CreateAndOpenDb();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<RamMetrics>("SELECT Id, Value FROM rammetrics").ToList();

            }

        }
        public IList<RamMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }
    }
}