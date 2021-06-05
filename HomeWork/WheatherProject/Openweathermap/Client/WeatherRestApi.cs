using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Openweathermap.Client
{
    internal class WeatherRestApi
    {

        private readonly HttpClient _client;

        private readonly string _accessKey;


        public WeatherRestApi(string accessKey, int apiTimeoutSec, string baseUri)
        {
            _accessKey = accessKey;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseUri),
                Timeout = TimeSpan.FromSeconds(apiTimeoutSec)
            };
        }

        public async Task<dynamic> CallApiAsync<T>(string apiCommand, RequestType requestType,
           Dictionary<string, string> args, bool getAsBinary = false)
        {
            HttpContent httpContent = null;

            if (requestType == RequestType.Post)
            {
                if (args != null && args.Any())
                {
                    httpContent = new FormUrlEncodedContent(args);
                }
            }

            try
            {
                var relativeUrl = apiCommand;
                if (requestType == RequestType.Get)
                {
                    if (args != null && args.Any())
                    {
                        relativeUrl += "?" + UrlEncodeParams(args);
                    }
                }

                using (var request = new HttpRequestMessage(
                    requestType == RequestType.Get ? HttpMethod.Get : HttpMethod.Post,
                    new Uri(_client.BaseAddress, relativeUrl)
                    ))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = httpContent;

                    var response = await _client.SendAsync(request).ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        var resultAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var json = JsonConvert.DeserializeObject<dynamic>(resultAsString);
                        WeatherException.ThrowException(apiCommand, json);
                    }

                    if (getAsBinary)
                    {
                        var resultAsByteArray = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        return resultAsByteArray;
                    }
                    else
                    {
                        var resultAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var json = JsonConvert.DeserializeObject<T>(resultAsString);
                        return json;
                    }
                }
            }
            finally
            {
                httpContent?.Dispose();
            }
        }

        #region Private methods

        private static string UrlEncodeString(string text)
        {
            var result = text == null ? "" : Uri.EscapeDataString(text).Replace("%20", "+");
            return result;
        }

        private static string UrlEncodeParams(Dictionary<string, string> args)
        {
            var sb = new StringBuilder();

            var arr =
                args.Select(
                    x =>
                        string.Format(CultureInfo.InvariantCulture, "{0}={1}", UrlEncodeString(x.Key), UrlEncodeString(x.Value))).ToArray();

            sb.Append(string.Join("&", arr));
            return sb.ToString();
        }
        #endregion
    }
}
