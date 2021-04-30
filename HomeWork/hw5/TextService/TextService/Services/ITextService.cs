using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TextService.Entities;

namespace TextService.Services
{
    public interface ITextService
    {
        Task<TextFile> AddFile(string text);
        Task<TextFile> AddFileByUrl(string textFileUrl, CancellationToken cancellationToken);
        Task<TextFile> UploadFile(IFormFile file);
        Task<TextFile> GetById(Guid id);
        Task<IEnumerable<TextFile>> GetAll();
    }
}