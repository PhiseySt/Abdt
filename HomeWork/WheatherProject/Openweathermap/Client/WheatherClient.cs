using AutoMapper;
using Microsoft.Extensions.Logging;
using Openweathermap.Helpers;
using Openweathermap.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Openweathermap.Client
{
    public class WeatherClient
    {
        private const int DefaultApiTimeoutSec = 10;
        private const string DefaultBaseUri = "https://api.openweathermap.org/";
        private readonly WeatherRestApi _restApi;
        private string m_accessKey ;
        private class NameValueDictionary : Dictionary<string, string> { }
        public WeatherClient(string accessKey, int apiTimeoutSec = DefaultApiTimeoutSec)
        {
            _restApi = new WeatherRestApi(accessKey, apiTimeoutSec, DefaultBaseUri);
            m_accessKey = accessKey;
        }

        protected virtual async Task<dynamic> CallApiAsync<T>(string apiCommand, RequestType requestType = RequestType.Get, Dictionary<string, string> args = null)
        {
            return await _restApi.CallApiAsync<T>(apiCommand, requestType, args).ConfigureAwait(false);
        }

        public async Task<T> GetCurrentWeatherSourceDataAsync<T>(string cityName, string lang, string units = "metric", string version = "2.5")
        {
            var args = new NameValueDictionary
            {
                {"q", cityName},
                {"lang", lang},
                {"units", units},
            };

           args.Add("appid", m_accessKey.ToString(CultureInfo.InvariantCulture));


            return await CallApiAsync<T>($"/data/{version}/weather/", RequestType.Get, args).ConfigureAwait(false);
        }


        public async Task<T> GetFiveDayForecastApiAsync<T>(string cityName, string lang, string units = "metric", string version = "2.5")
        {
            var args = new NameValueDictionary
            {
                {"q", cityName},
                {"lang", lang},
                {"units", units},
            };

            args.Add("appid", m_accessKey.ToString(CultureInfo.InvariantCulture));

            return await CallApiAsync<T>($"/data/{version}/forecast", RequestType.Get, args).ConfigureAwait(false);
        }

        public async Task<WeatherModel> GetCurrentWeatherResultDataAsync<T>(ILogger<T> logger, string cityName, string metric, OpenWeatherMapModel resourceData)
        {
            var resultData = new WeatherModel();
            await Task.Run(() =>
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<OpenWeatherMapModel, LogInfo>());
                var mapper = new Mapper(config);
                var logInfo = mapper.Map<LogInfo>(resourceData);
                logger.LogInformation(logInfo.ToString());
                var resultTemperature = metric.Equals("celsius") ? resourceData.Main.Temp : MetricHelper.CToF(resourceData.Main.Temp);
                resultData = new WeatherModel
                {
                    City = cityName,
                    Temperature = resultTemperature,
                    Date = DateTime.Now.ToString(),
                    TemperatureMetric = metric
                };
            });
            return resultData;
        }

        public async Task<WindModel> GetCurrentWindResultDataAsync<T>(ILogger<T> logger, string cityName, OpenWeatherMapModel resourceData)
        {
            var resultData = new WindModel();
            await Task.Run(() =>
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<OpenWeatherMapModel, LogInfo>());
                var mapper = new Mapper(config);
                var logInfo = mapper.Map<LogInfo>(resourceData);
                logger.LogInformation(logInfo.ToString());
                resultData = new WindModel
                {
                    City = cityName,
                    Speed = resourceData.Wind.Speed,
                    Direction = WindDirectionHelper.FormatWindDirection(resourceData.Wind.WindDirection)
                };
            });
            return resultData;
        }

        public async Task<IEnumerable<WeatherModel>> GetWeather5DaysResultDataAsync<T>(ILogger<T> logger, string cityName, string metric, WeatherFiveDaysData resourceData)
        {
            var weatherFiveDaysModel = new WeatherFiveDaysModel
            {
                WeatherFiveDays = new List<WeatherModel>()
            };
            await Task.Run(() =>
            {
                var listWheather = resourceData.list;
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
            });
            return weatherFiveDaysModel.WeatherFiveDays;
        }
    }
}
