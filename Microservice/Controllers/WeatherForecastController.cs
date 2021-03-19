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

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet]
        public WeatherForecast Get()
        {
            var rng = new Random();

          
            var WeatherForecast = new WeatherForecast();     
            double index = rng.NextDouble();

            DateTime DateTime = DateTime.Now;

            WeatherForecast.Date = DateTime.AddDays(index).ToString("f");
            WeatherForecast.TemperatureC = rng.Next(-20, 55);
            WeatherForecast.Summary = Summaries[rng.Next(Summaries.Length)];
           
            return WeatherForecast;
        }
        
    }
}
