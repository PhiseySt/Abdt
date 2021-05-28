using Microsoft.AspNetCore.Http;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TextService.Entities.Models;

namespace TextService.Client
{
    public interface ITextClient
    {
   
        [Get("/api/textservice/{id}")]
        Task<TextFile> GetById(Guid id);

        [Get("/api/textservice/getall")]
        Task<IEnumerable<TextFile>> GetAllTexts();

        [Post("/api/textservice")]
        Task<TextFile> Post([Body] string text);

        [Post("/api/textservice/upload")]
        Task<TextFile> PostFile(IFormFile streamTextFile);        

        [Post("/api/textservice/url")]
        Task<TextFile> PostFileUrl([Body] string fileUrl, CancellationToken cancellationToken);

   }
}
