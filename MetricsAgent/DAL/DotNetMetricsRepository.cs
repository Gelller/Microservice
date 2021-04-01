﻿using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetrics>
    {

    }

    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        // наше соединение с базой данных
        private SQLiteConnection _connection;

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public DotNetMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }    
            public void Create(DotNetMetrics item)
            {
                // создаем команду
                using var cmd = new SQLiteCommand(_connection);
                // прописываем в команду SQL запрос на вставку данных
                cmd.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";

                // добавляем параметры в запрос из нашего объекта
                cmd.Parameters.AddWithValue("@value", item.Value);

                // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
                // через свойство
                cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
                // подготовка команды к выполнению
                cmd.Prepare();

                // выполнение команды
                cmd.ExecuteNonQuery();
            }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(_connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = "DELETE FROM cpumetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(DotNetMetrics item)
        {
            using var cmd = new SQLiteCommand(_connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<DotNetMetrics> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM cpumetrics";

            var returnList = new List<DotNetMetrics>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new DotNetMetrics
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        // налету преобразуем прочитанные секунды в метку времени
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                    });
                }
            }

            return returnList;
        }
        public IList<DotNetMetrics> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var cmd = new SQLiteCommand(_connection);
            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM cpumetrics";
            var returnList = new List<DotNetMetrics>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    if (reader.GetInt32(2) >= fromTime.ToUnixTimeSeconds() && reader.GetInt32(2) <= toTime.ToUnixTimeSeconds())
                    {
                        // добавляем объект в список возврата
                        returnList.Add(new DotNetMetrics
                        {
                            Id = reader.GetInt32(0),
                            Value = reader.GetInt32(1),
                            // налету преобразуем прочитанные секунды в метку времени                     
                            Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                        });
                    }
                }
            }

            return returnList;
        }
        public DotNetMetrics GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = "SELECT * FROM cpumetrics WHERE id=@id";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new DotNetMetrics
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                    };
                }
                else
                {
                    // не нашлось запись по идентификатору, не делаем ничего
                    return null;
                }
            }
        }
    }
}
