using FindService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextService.Client;
using TextService.Entities.Models;

namespace FindService.Services.Services
{
    public class FindService : IFindService
    {
        private readonly ITextClient _textClient;
        public FindService(ITextClient textClient)
        {
            _textClient = textClient;
        }       
        public async Task<IEnumerable<TextFile>> FindWordAsync(string word)
        {
            if (word != null)
            {
                var getText = await _textClient.GetAllTexts();
                if (getText != null)
                {
                    var selectText = getText.Where(x => x.TextValue.Contains(word));
                    return selectText;
                }
            }

            return null;
        }        
        public async Task<IEnumerable<string>> FindWordsAsync(Guid id, string[] words)
        {
            if (words != null)
            {
                var getText = await _textClient.GetById(id);
                if (getText != null)
                {
                    var textArray = getText.TextValue.Replace('\r', ' ').Replace('\n', ' ').Replace("  ", " ").Split(" ");
                    var selectText = textArray.SelectMany(e => words.Where(x => x == e));
                    return selectText;
                }
            }

            return null;
        }
        public async Task<IEnumerable<TextFile>> GetAllFilesAsync()
        {
            try
            {
                var getText = await _textClient.GetAllTexts();
                return getText;
            }           

            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
