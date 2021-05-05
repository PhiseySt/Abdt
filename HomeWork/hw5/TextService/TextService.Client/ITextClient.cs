using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Microsoft.AspNetCore.Http;
using TextService.Entities;

namespace TextService.Client
{
    public interface ITextClient
    {
        [Get("/text/{id}")]
        Task<TextFile> GetById(Guid id);
        [Get("/text/getall")]
        Task<IEnumerable<TextFile>> GetAll();
        [Post("/text")]
        Task<TextFile> Post([Body] string text);
        [Post("/text/url")]
        Task<TextFile> AddFileByUrl(string textFileUrl, CancellationToken cancellationToken);
        [Post("/text/upload")]
        Task<TextFile> UploadFile(IFormFile file);
    }
}
