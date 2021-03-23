using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private WeatherList _weatherList;
        public CrudController(WeatherList weatherList)
        {        
            this._weatherList = weatherList;
        }
        //сохранить температуру в указанное время
        [HttpPost("InputTemperature")]
        public IActionResult InputTemperature([FromQuery] DateTime inputDateTemperature, [FromQuery] int temperature)
        {
            var newWeather = new WeatherForecast();       
            newWeather.Date = inputDateTemperature;
            newWeather.TemperatureC = temperature;
            _weatherList.Values.Add(newWeather);
            return Ok();
        }
        //отредактировать показатель температуры в указанное время
        [HttpPut("EditTemperature")]
        public IActionResult EditTemperature([FromQuery] DateTime inputDateTemperature, [FromQuery] int inputTemperature)
        {
            foreach (var item in _weatherList.Values)
            {
                if (inputDateTemperature == item.Date)
                    item.TemperatureC = inputTemperature;
            }
            return Ok();
        }
        //удалить показатель температуры в указанный промежуток времени
        [HttpDelete("DeleteTemperature")]
        public IActionResult DeleteTemperature([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            List<WeatherForecast> itemsToDelete = new List<WeatherForecast>();
            foreach (var item in _weatherList.Values)
            {
                if (dateFrom <= item.Date.Date && dateTo >= item.Date.Date)
                    itemsToDelete.Add(item);
            }
            foreach (var item in itemsToDelete)
            {
                _weatherList.Values.Remove(item);
            }
            return Ok();
        }
        //прочитать список показателей температуры за указанный промежуток времени
        [HttpGet("ReadTemperature")]
        public IActionResult ReadTemperature([FromQuery] DateTime inputDatefrom, [FromQuery] DateTime inputDateTo)
        {
            List<WeatherForecast> itemOutput = new List<WeatherForecast>();
            foreach (var item in _weatherList.Values)
            {
                if (inputDatefrom <= item.Date.Date && inputDateTo >= item.Date.Date)
                    itemOutput.Add(item);
            }
            return Ok(itemOutput);
        }
    }
}
