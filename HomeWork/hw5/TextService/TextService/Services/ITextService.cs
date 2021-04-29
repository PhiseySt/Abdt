using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TextService.Entities;

namespace TextService.Services
{
    public interface ITextService
    {
        Task<TextFile> AddFile(string text);
        Task<TextFile> GetById(Guid id);
    }
}