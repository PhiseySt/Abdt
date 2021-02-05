namespace YandexUploadFiles.Loader.Models
{
    internal class UserInfo
    {
            public long Total_Space { get; set; }

            public long Used_Space { get; set; }

            public YandexUser User { get; set; }
    }

    internal class YandexUser
    {
        public string Login { get; set; }
        public string Display_Name { get; set; }
    }
}
