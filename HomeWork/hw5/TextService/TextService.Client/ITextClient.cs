using System;
using System.IO;
using System.Threading.Tasks;
using Refit;
using TextService.Entities;

namespace TextService.Client
{
    public interface ITextClient
    {
        [Get("/text/{id}")]
        Task<TextFile> GetById(Guid id);
        [Post("/text")]
        Task<TextFile> Post([Body] string text);
        [Post("/text/file/{streamTextFile}")]
        Task<TextFile> PostFile(Stream streamTextFile);
        [Post("/text/url/{fileUrl}")]
        Task<TextFile> PostFileUrl([Body]string fileUrl);
    }
}
