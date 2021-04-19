using System;
using System.Collections.Generic;

namespace MetricsAgent
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();

        void Create(T item);

        IList<T> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime);
    }
}
