using System;
using System.Collections.Generic;

namespace MetricsAgent
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();

        T GetById(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

        IList<T> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime);
    }
}
