using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TextService.Entities.Models;
using System.Threading;

namespace TextService.Services.Interfaces
{
    public interface ITextService
    {
        Task<IEnumerable<TextFile>> GetAllAsync();
        Task<ActionResult<TextFile>> GetByIdAsync(Guid id);
        Task<TextFile> AddFileByUrlAsync(string textFileUrl, CancellationToken cancellationToken);
        Task<TextFile> AddFileAsync(string text);
        Task<TextFile> UploadFileAsync(IFormFile file);


    }
}