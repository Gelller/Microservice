using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        [HttpGet]
        public WeatherForecast Get()
        {
            var rng = new Random();
            var WeatherForecast = new WeatherForecast();     

            DateTime DateTime = DateTime.Now;
            DateTime newDate = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day);

            WeatherForecast.Date = newDate.Date.AddDays(rng.Next(0, 100));
            WeatherForecast.TemperatureC = rng.Next(-20, 55);
            WeatherForecast.Summary = Summaries[rng.Next(Summaries.Length)];
           
            return WeatherForecast;
        }    
    }
}
