namespace YandexUploadFiles.Loader.Models
{
    internal interface IUploadProgress
    {
        public interface IUploadProgress
        {
            void UpdateProgress(long current, long total, string filename);
        }
    }
}