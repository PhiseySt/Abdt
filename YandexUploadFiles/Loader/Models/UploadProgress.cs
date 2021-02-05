using System;

namespace YandexUploadFiles.Loader.Models
{
    internal class UploadProgress : IUploadProgress
    {
            private readonly Action<long, long, string> _progressAction;

            public UploadProgress(Action<long, long, string> progressAction)
            {
                _progressAction = progressAction;
            }

            public void UpdateProgress(long current, long total, string filename)
            {
                _progressAction(current, total, filename);
            }

    }
}
