using Openweathermap.Models;
using Refit;
using System.Collections.Generic;

namespace Openweathermap.Client
{
    public interface IOpenweathermapClient
    {
        [Get("weather/temperature/{cityname}/{metric}")]
        public WeatherModel GetWeatherTemperature(string cityName = "Kazan", string metric = "celsius");

        [Get("weather/wind/{cityName}")]
        WindModel GetWeatherWind(string cityName = "Kazan");

        [Get("weather/feature/{cityname}/{metric}")]
        IEnumerable<WeatherModel> GetWeatherTemperatures5Days(string cityName = "Kazan", string metric = "celsius");
    }
}
