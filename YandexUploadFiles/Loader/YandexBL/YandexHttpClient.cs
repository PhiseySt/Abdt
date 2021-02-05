using System;
using System.Net.Http;


namespace YandexUploadFiles.Loader.YandexBl
{
    internal class YandexHttpClient : HttpClient
    {
        public YandexHttpClient(string token)
        {
            var connectTimeout = new TimeSpan(0, 0, 30);
            Timeout = connectTimeout;
            DefaultRequestHeaders.Add("Accept", "application/json");
            DefaultRequestHeaders.Add("Authorization", $"OAuth {token}");
        }
    }
}
