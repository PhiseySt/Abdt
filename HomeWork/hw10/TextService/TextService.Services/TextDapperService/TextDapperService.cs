﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TextService.Entities.Models;
using TextService.Repositories.Interfaces;
using TextService.Services.Interfaces;

namespace TextService.Services.TextDapperService
{
    public class TextDapperService : ITextService
    {
        private readonly ITextDapperRepository _textRepository;
        private readonly IMapper _mapper;

        public TextDapperService(ITextDapperRepository textRepository, IMapper mapper)
        {
            _textRepository = textRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TextFile>> GetAllAsync()
        {
            var texts = await _textRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TextFile>>(texts);
        }

        public async Task<ActionResult<TextFile>> GetByIdAsync(Guid id)
        {
            var text = await _textRepository.GetByIdAsync(id);
            return _mapper.Map<TextFile>(text);
        }

        public async Task<TextFile> AddFileByUrlAsync(string textFileUrl, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(textFileUrl, cancellationToken);
            using var sr = new StreamReader(stream);
            var fileData = sr.ReadToEnd();
            var textFile = new Repositories.Entities.TextEntity { TextValue = fileData };

            textFile = await _textRepository.CreateAsync(textFile);
            textFile.TextValue = null;

            return _mapper.Map<TextFile>(textFile);

        }

        public async Task<TextFile> UploadFileAsync(IFormFile file)
        {
            var pathLocalCopy = await WriteFileLocalStorage(file);
            return await AddFileUpload(pathLocalCopy);
        }

        public async Task<TextFile> AddFileAsync(string text)
        {
            var textFile = new TextService.Repositories.Entities.TextEntity { TextValue = text };

            textFile = await _textRepository.CreateAsync(textFile);
            textFile.TextValue = null;

            return _mapper.Map<TextFile>(textFile);
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
            var textFile = new TextService.Repositories.Entities.TextEntity { TextValue = fileData };
            textFile = await _textRepository.CreateAsync(textFile);
            textFile.TextValue = null;
            return _mapper.Map<TextFile>(textFile);
        }
    }
}