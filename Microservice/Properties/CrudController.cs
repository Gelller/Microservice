using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace Microservice.Properties
{

    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private WeatherList _weatherList;
        public CrudController(WeatherList WeatherList)
        {        
            this._weatherList = WeatherList;
        }
        //сохранить температуру в указанное время
        [HttpPost("InputTemperature")]
        public IActionResult InputTemperature([FromQuery] DateTime inputDateTemperature)
        {
            int SaveTemp=0;
            WeatherForecast bufWeather = null;
            foreach (var item in _weatherList.Values)
            {
                if (inputDateTemperature.Date == item.Date.Date)
                {
                    SaveTemp = item.TemperatureC;
                    bufWeather = item;
                }
            }
            return Ok("TemperatureC "+SaveTemp+ "C");
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
            return Ok(_weatherList);
        }
        //удалить показатель температуры в указанный промежуток времени
        [HttpDelete("DeleteTemperature")]
        public IActionResult DeleteTemperature([FromQuery] DateTime DateFrom, [FromQuery] DateTime DateTo)
        {
            List<WeatherForecast> delList = new List<WeatherForecast>();
            foreach (var item in _weatherList.Values)
            {
                if (DateFrom <= item.Date.Date && DateTo >= item.Date.Date)
                    delList.Add(item);
            }
            foreach (var item in delList)
            {
                _weatherList.Values.Remove(item);
            }
            return Ok(_weatherList);
        }
        //прочитать список показателей температуры за указанный промежуток времени
        [HttpGet("ReadTemperature")]
        public IActionResult ReadTemperature([FromQuery] DateTime inputDatefrom, [FromQuery] DateTime inputDateTo)
        {
            List<WeatherForecast> ListOutput = new List<WeatherForecast>();
            foreach (var item in _weatherList.Values)
            {
                if (inputDatefrom <= item.Date.Date && inputDateTo >= item.Date.Date)
                    ListOutput.Add(item);
            }
            return Ok(ListOutput);
        }
        //чтение списка
        [HttpGet("Read")]
        public IActionResult Read()
        {        
            return Ok(_weatherList);
        }
        //создание списка температур
        [HttpPost("Create")]
        public IActionResult Create()
        {        
            var RandomDate = new Controllers.WeatherForecastController();
            for (int i = 0; i < 5; i++)
            {
                _weatherList.Values.Add(RandomDate.Get()); 
            }
            return Ok(_weatherList);
        }

    }
}
