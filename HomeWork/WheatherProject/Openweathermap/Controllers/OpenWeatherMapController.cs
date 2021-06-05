using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Openweathermap.Client;
using Openweathermap.Helpers;
using Openweathermap.Models;
using System;
using System.Collections.Generic;


namespace Openweathermap.Controllers
{
    [ApiController]
    [Route("Weather/")]
    public class OpenWeatherMapController : ControllerBase
    {
        private string m_accessKey;

        private IConfiguration _configuration;
        private readonly ILogger<OpenWeatherMapController> _logger;

        public OpenWeatherMapController(ILogger<OpenWeatherMapController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            m_accessKey = _configuration["AccessKey"];
        }

        /// <summary>
        /// Получить температуру
        /// </summary>
        [Route("Temperature/{cityName}/{metric}")]
        [HttpGet]
        public WeatherModel GetWeatherTemperature(string cityName="Kazan", string metric = "celsius")
        {
            WeatherClient client = new WeatherClient(m_accessKey);
            var resourceData = client.GetCurrentWeatherAsync<OpenWeatherMapModel>(cityName, "en").Result;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OpenWeatherMapModel, LogInfo>());
            var mapper = new Mapper(config);
            var logInfo = mapper.Map<LogInfo>(resourceData);
            _logger.LogInformation(logInfo.ToString());
            var resultTemperature = metric.Equals("celsius") ? resourceData.Main.Temp : MetricHelper.CToF(resourceData.Main.Temp);
            var resultData = new WeatherModel
            {
                City = cityName,
                Temperature = resultTemperature,
                Date = DateTime.Now.ToString(),
                TemperatureMetric = metric
            };
            return resultData;
        }

        /// <summary>
        /// Получить информацию о ветре
        /// </summary>
        [Route("Wind/{cityName}")]
        [HttpGet]
        public WindModel GetWeatherWind(string cityName = "Kazan")
        {
            WeatherClient client = new WeatherClient(m_accessKey);
            OpenWeatherMapModel resourceData = new OpenWeatherMapModel();
            resourceData = client.GetCurrentWeatherAsync<OpenWeatherMapModel>(cityName, "en").Result;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OpenWeatherMapModel, LogInfo>());
            var mapper = new Mapper(config);
            var logInfo = mapper.Map<LogInfo>(resourceData);
            _logger.LogInformation(logInfo.ToString());
            var resultData = new WindModel
            {
                City = cityName,
                Speed = resourceData.Wind.Speed,
                Direction = WindDirectionHelper.FormatWindDirection(resourceData.Wind.WindDirection)
            };
            return resultData;
        }

        /// <summary>
        /// Получить температуру за 5 дней
        /// </summary>
        [Route("Feature/{cityName}/{metric}")]
        [HttpGet]
        public IEnumerable<WeatherModel> GetWeatherTemperatures5Days(string cityName = "Kazan", string metric = "celsius")
        {
   
            WeatherClient client = new WeatherClient(m_accessKey);
            var resourceData = client.GetFiveDayForecastApiAsync<WeatherFiveDaysData>(cityName, "en").Result;
            var listWheather = resourceData.list;
            var weatherFiveDaysModel = new WeatherFiveDaysModel
            {
                WeatherFiveDays = new List<WeatherModel>()
            };

            foreach (var itemWheather in listWheather) 
            {
                var resultTemperature = metric.Equals("celsius") ? itemWheather.main.Temp : MetricHelper.CToF(itemWheather.main.Temp);

                var resultData = new WeatherModel
                {
                    City = cityName,
                    Temperature = resultTemperature,
                    Date = itemWheather.dt_txt,
                    TemperatureMetric = metric
                };
                weatherFiveDaysModel.WeatherFiveDays.Add(resultData);
            }
            return weatherFiveDaysModel.WeatherFiveDays;
        }
    }
}
