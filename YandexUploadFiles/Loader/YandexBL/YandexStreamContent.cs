using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace YandexUploadFiles.Loader.YandexBL
{
    public delegate void YandexProgressHandler(long bytes, long currentBytes, long totalBytes);

    internal class YandexStreamContent : StreamContent
    {
        private const int DefaultBufferSize = 4 * 1024;

        public event YandexProgressHandler ProgressChanged = delegate { };

        private long _currentBytes;
        private long _allBytes = -1;


        public Stream InnerStream { get; }
        public int BufferSize { get; }

        public YandexStreamContent(Stream innerStream, int bufferSize = DefaultBufferSize) :
            base(innerStream, bufferSize)
        {
            InnerStream = innerStream;
            BufferSize = bufferSize;
        }

        private void ResetInnerStream()
        {
            if (InnerStream.Position == 0) return;

            if (InnerStream.CanSeek)
            {
                InnerStream.Position = 0;
                _currentBytes = 0;
            }
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            ResetInnerStream();

            if (_allBytes == -1)
                _allBytes = Headers.ContentLength ?? -1;


            if (_allBytes == -1 && TryComputeLength(out var computedLength))
                _allBytes = computedLength == 0 ? -1 : computedLength;

            _allBytes = Math.Max(-1, _allBytes);

            var buffer = new byte[BufferSize];
            int bytesRead;
            while ((bytesRead = await InnerStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                stream.Write(buffer, 0, bytesRead);
                _currentBytes += bytesRead;

                ProgressChanged(bytesRead, _currentBytes, _allBytes);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            var result = base.TryComputeLength(out length);
            _allBytes = length;
            return result;
        }

    }
}
