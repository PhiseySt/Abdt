using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextService.Entities.Models;

namespace FindService.Client
{
    public interface IFindClient
    {
        [Get("/api/findservice")]
        Task<IEnumerable<TextFile>> GetAllTextsAsync();
        [Get("/api/findservice/existword")]
        Task<string> IsExistWordAsync(string word);
        [Get("/api/findservice/textbymask")]
        Task<string> GetWordsByMaskAsync(Guid id, string[] setWords);

    }
}
