using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextService.Entities.Models;

namespace FindService.Services.Interfaces
{
    public interface IFindService
    {
        /// <summary>
        /// Поиск слова во всех файлах
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        Task<IEnumerable<TextFile>> FindWordAsync(string word);

        /// <summary>
        /// Поиск слов в указанном файле
        /// </summary>
        /// <param name="id"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> FindWordsAsync(Guid id, string[] words);

        /// <summary>
        /// Получаем все файлы
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TextFile>> GetAllFilesAsync();
    }
}
