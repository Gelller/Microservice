using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice
{
    public class WeatherList
    {
        public List<WeatherForecast> Values { get; set; }
        public WeatherList()
        {
            Values = new List<WeatherForecast>();
        }
    }
}
