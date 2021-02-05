using System;

namespace YandexUploadFiles.Loader.YandexBl
{
    internal static class MeasureUnit
    {
        public static double BytesToKilobytes(long bytes)
        {
            return Math.Round(bytes / 1024d, 4);
        }
        public static double BytesToMegabytes(long bytes)
        {
            return Math.Round(bytes / 1024d / 1024d, 4);
        }
        public static double BytesToGigabytes(long bytes)
        {
            return Math.Round(bytes / 1024d / 1024d / 1024d, 4);
        }
    }
}
