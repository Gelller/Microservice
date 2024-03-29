﻿using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.DAL.Interfaces;
using Dapper;
using System.Linq;
using System.Data;
using System.IO;
using MetricsAgent.Controllers;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.DAL.Repository
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly ILogger<RamMetricsController> _logger;
        public RamMetricsRepository(ILogger<RamMetricsController> logger)
        {
            _logger = logger;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(RamMetrics item)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
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
                using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
                {
                    return connection.Query<RamMetrics>("SELECT Id, Value, Time FROM rammetrics").ToList();
                }      
        }
        public IList<RamMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                return connection.Query<RamMetrics>($"SELECT Id, Value, Time FROM rammetrics WHERE Time>={fromTime.ToUnixTimeSeconds()} AND Time<={toTime.ToUnixTimeSeconds()}").ToList();
            }
        }     
    }
}
