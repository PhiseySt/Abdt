using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Text.Service.Repositories;
using TextService.Entities;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace TextService.Services
{
    public class TextService : ITextService
    {
        private readonly ITextRepository _textRepository;
        private readonly IMapper _mapper;

        public TextService(ITextRepository textRepository,
        IMapper mapper)
        {
            _textRepository = textRepository;
            _mapper = mapper;
        }

        public async Task<TextFile> AddFile(string text)
        {
            var textFile = new Text.Service.Repositories.Text { TextValue = text };

            textFile = await _textRepository.Create(textFile);
            textFile.TextValue = null;

            return _mapper.Map<TextFile>(textFile);
        }

        public async Task<TextFile> AddFileByUrl(string textFileUrl, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(textFileUrl, cancellationToken);
            using var sr = new StreamReader(stream);
            var fileData = sr.ReadToEnd();
            var textFile = new Text.Service.Repositories.Text { TextValue = fileData };

            textFile = await _textRepository.Create(textFile);
            textFile.TextValue = null;

            return _mapper.Map<TextFile>(textFile);

        }

        public async Task<TextFile> UploadFile(IFormFile file)
        {
            var pathLocalCopy = await WriteFileLocalStorage(file);
            return await AddFileUpload(pathLocalCopy);
        }

        private async Task<string> WriteFileLocalStorage(IFormFile file)
        {
            string path;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extension;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                    fileName);
                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }
            catch (Exception e)
            {
                throw;
            }

            return path;
        }

        private async Task<TextFile> AddFileUpload(string fileName)
        {
            byte[] result;
            await using (FileStream sourceStream = File.Open(fileName, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
            }

            var fileData = System.Text.Encoding.UTF8.GetString(result);
            var textFile = new Text.Service.Repositories.Text { TextValue = fileData };
            textFile = await _textRepository.Create(textFile);
            textFile.TextValue = null;
            return _mapper.Map<TextFile>(textFile);
        }



        public async Task<TextFile> GetById(Guid id)
        {
            var text = await _textRepository.GetById(id);
            return _mapper.Map<TextFile>(text);
        }

        public async Task<IEnumerable<TextFile>> GetAll()
        {
            var texts = await _textRepository.GetAll();
            return _mapper.Map<IEnumerable<TextFile>>(texts);
        }

    }
}