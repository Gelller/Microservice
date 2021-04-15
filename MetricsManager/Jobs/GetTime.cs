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
    public class GetTime
    {
        public GetTime()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }      
        public CpuMetrics GetTimeCpu(string table)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {            
            var time=connection.Query<CpuMetrics>($"SELECT Id, Time, Value, AgentId FROM {table}").ToList();

                if (time.Count != 0)
                {
                    var bufferTime = time[0];
                    foreach (var item in time)
                    {
                        if (item.Time > bufferTime.Time)
                            bufferTime.Time = item.Time;
                    }
                    return bufferTime;
                }
                else
                    return null;
            }

        }
        public DotNetMetrics GetTimeDotNet(string table)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                var time = connection.Query<DotNetMetrics>($"SELECT Id, Time, Value, AgentId FROM {table}").ToList();

                if (time.Count != 0)
                {
                    var bufferTime = time[0];
                    foreach (var item in time)
                    {
                        if (item.Time > bufferTime.Time)
                            bufferTime.Time = item.Time;
                    }
                    return bufferTime;
                }
                else
                    return null;
            }
        }
        public HddMetrics GetTimeHdd(string table)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                var time = connection.Query<HddMetrics>($"SELECT Id, Time, Value, AgentId FROM {table}").ToList();

                if (time.Count != 0)
                {
                    var bufferTime = time[0];
                    foreach (var item in time)
                    {
                        if (item.Time > bufferTime.Time)
                            bufferTime.Time = item.Time;
                    }
                    return bufferTime;
                }
                else
                    return null;
            }
        }
        public NetworkMetrics GetTimeNetwork(string table)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                var time = connection.Query<NetworkMetrics>($"SELECT Id, Time, Value, AgentId FROM {table}").ToList();

                if (time.Count != 0)
                {
                    var bufferTime = time[0];
                    foreach (var item in time)
                    {
                        if (item.Time > bufferTime.Time)
                            bufferTime.Time = item.Time;
                    }
                    return bufferTime;
                }
                else
                    return null;
            }
        }
        public RamMetrics GetTimeRam(string table)
        {
            using (var connection = new SQLiteConnection(SQLConnected.ConnectionString))
            {
                var time = connection.Query<RamMetrics>($"SELECT Id, Time, Value, AgentId FROM {table}").ToList();

                if (time.Count != 0)
                {
                    var bufferTime = time[0];
                    foreach (var item in time)
                    {
                        if (item.Time > bufferTime.Time)
                            bufferTime.Time = item.Time;
                    }
                    return bufferTime;
                }
                else
                    return null;
            }
        }
    }
}
