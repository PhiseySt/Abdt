using Newtonsoft.Json;
using YandexUploadFiles.Loader.Models;

namespace YandexUploadFiles.Loader.YandexBl
{
    internal class YandexUploader
    {
        public string Token { get; }
        private string Url { get; }

        public YandexUploader(string token)
        {
            Token = token;
            Url = DefaultSettings.YandexDiskUrl;
        }

        public UserInfo GetUserInfo()
        {
            UserInfo userInfo = null;
            var client = new YandexHttpClient(Token);

            var task = client.GetAsync(Url).ContinueWith((requestTask) =>
            {
                var response = requestTask.Result;
                var json = response.Content.ReadAsStringAsync();
                json.Wait();
                userInfo = JsonConvert.DeserializeObject<UserInfo>(json.Result);
            });
            task.Wait();

            return userInfo;
         }

    }
}
