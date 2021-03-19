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

        private SaveTemp SaveTemp;
        
        private WeatherList WeatherList;
       
        public CrudController(WeatherList WeatherList, SaveTemp SaveTemp)
        {
        
            this.WeatherList = WeatherList;
            this.SaveTemp = SaveTemp;
        }

        //сохранить температуру в указанное время
        [HttpPost("inputTemp")]
        public IActionResult inputTemp([FromQuery] string inputDateTime)
        {
                
            WeatherForecast bufWeather = null;
            foreach (var item in WeatherList.Values)
            {
                if (inputDateTime == item.Date)
                {
                    SaveTemp.temp = item.TemperatureC;
                    bufWeather = item;
                }

            }

            return Ok("TemperatureC "+SaveTemp.temp+ "C");
        }

        //отредактировать показатель температуры в указанное время
        [HttpPost("editTemp")]
        public IActionResult editTemp([FromQuery] string inputDateTime,int inputTemp)
        {

            foreach (var item in WeatherList.Values)
            {
                if (inputDateTime == item.Date)
                    item.TemperatureC = inputTemp;

            }

            return Ok(WeatherList);
        }

        //удалить показатель температуры в указанный промежуток времени
        [HttpPost("deleteTemp")]
        public IActionResult deleteTemp([FromQuery] string input, string input2)
        {
          
            List<WeatherForecast> delList = new List<WeatherForecast>();
            
           
            DateTime datefrom = Convert.ToDateTime(input);
            DateTime dateto = Convert.ToDateTime(input2);

            foreach (var item in WeatherList.Values)
            {
                if (datefrom<= Convert.ToDateTime(item.Date)&& dateto >= Convert.ToDateTime(item.Date))
                    delList.Add(item);


            }

            foreach (var item in delList)
            {
                WeatherList.Values.Remove(item);
            }
            return Ok(WeatherList);
        }

        //прочитать список показателей температуры за указанный промежуток времени
        [HttpGet("readTemp")]
        public IActionResult readTemp([FromQuery] string inputDatefrom, string inputDateTo)
        {


            List<WeatherForecast> ListOutput = new List<WeatherForecast>();

            DateTime datefrom = Convert.ToDateTime(inputDatefrom);
            DateTime dateto = Convert.ToDateTime(inputDateTo);

            foreach (var item in WeatherList.Values)
            {
                if (datefrom <= Convert.ToDateTime(item.Date) && dateto >= Convert.ToDateTime(item.Date))
                    ListOutput.Add(item);


            }

            return Ok(ListOutput);
        }


        //чтение списка
        [HttpGet("read")]
        public IActionResult Read()
        {        
            return Ok(WeatherList);
        }

        //создание списка температур
        [HttpGet("create")]
        public IActionResult Create()
        {        
             var RandomDate = new Controllers.WeatherForecastController();
            for (int i = 0; i < 5; i++)
            {
                WeatherList.Values.Add(RandomDate.Get());
                
            }

            return Ok(WeatherList);
        }

    }
}
