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

        public async Task<T> GetCurrentWeatherAsync<T>(string cityName, string lang, string units = "metric", string version = "2.5")
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
    }
}
