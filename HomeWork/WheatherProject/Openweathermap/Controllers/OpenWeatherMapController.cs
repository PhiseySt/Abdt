using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Openweathermap.Client;
using Openweathermap.Models;
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
            var resourceData = client.GetCurrentWeatherSourceDataAsync<OpenWeatherMapModel>(cityName, "en").Result;
            var resultData = client.GetCurrentWeatherResultDataAsync(_logger, cityName, metric, resourceData);
            return resultData.Result;
        }

        /// <summary>
        /// Получить информацию о ветре
        /// </summary>
        [Route("Wind/{cityName}")]
        [HttpGet]
        public WindModel GetWeatherWind(string cityName = "Kazan")
        {
            WeatherClient client = new WeatherClient(m_accessKey);
            var resourceData = client.GetCurrentWeatherSourceDataAsync<OpenWeatherMapModel>(cityName, "en").Result;
            var resultData = client.GetCurrentWindResultDataAsync(_logger, cityName, resourceData);
            return resultData.Result;
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
            var resultData = client.GetWeather5DaysResultDataAsync(_logger, cityName, metric, resourceData);
            return resultData.Result;
        }
    }
}
