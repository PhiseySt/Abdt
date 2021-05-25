using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextService.Client;
using TextService.Entities.Models;

namespace FindService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindServiceController : ControllerBase
    {
        private readonly ITextClient _textClient;
        private readonly ILogger<FindServiceController> _logger;

        public FindServiceController(
            ITextClient textClient,
            ILogger<FindServiceController> logger)
        {
            _textClient = textClient;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<TextFile>> GetAllTextsAsync()
        {
            try
            {
                var bearerToken = Request.Headers["Authorization"];
                _logger.LogInformation($"GetAllTextsAsync {bearerToken} {DateTime.Now}");
                var getText = await _textClient.GetAllTexts();
                return getText;
            }

            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Проверка существует ли слово в тексте
        /// </summary>
        /// <param name="word">Слово для поиска</param>
        [Authorize]
        [Route("ExistWord")]
        [HttpGet]
        public async Task<string> IsExistWordAsync([FromQuery] string word)
        {

            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"IsExistWordAsync {bearerToken} {DateTime.Now}");
            var getTexts = await _textClient.GetAllTexts();
            if (getTexts != null)
            {
                if (string.IsNullOrEmpty(word)) return "Слово не найдено при поиске";

                foreach (var currentText in getTexts)
                {
                    if (currentText.TextValue.Contains(word)) return "Слово найдено при поиске";
                }

            }
            return "Слово не найдено при поиске";
        }

        /// <summary>
        /// Подсчет количества слов в тексте по заданному id из принимаемого массива слов
        /// </summary>
        /// <param name="id">Универсальный идентификатор текста</param>
        /// <param name="setWords">Набор слов для поиска</param>
        [Authorize]
        [Route("TextByMask")]
        [HttpGet]
        public async Task<string> GetWordsByMaskAsync(Guid id, [FromQuery] string[] setWords)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetWordsByMaskAsync {bearerToken} {DateTime.Now}");
            if (setWords != null)
            {
                var token = Request.Headers["Authorization"];
                var setWordsParse = new string[] { };
                if (setWords.Length==1 && setWords[0].Contains(",")) setWordsParse= setWords[0].Split(",");
                var textFromClient = await _textClient.GetById(id);
                if (textFromClient != null)
                {
                    if (setWords.Length == 0) return "Количество совпадений: 0";

                    var wordsFromText = textFromClient.TextValue.Split(" ");
                    var countMatches = setWordsParse.Length>0 ? setWordsParse.Sum(word => wordsFromText.Count(wft => wft == word))
                        : setWords.Sum(word => wordsFromText.Count(wft => wft == word));

                    return $"Количество совпадений: {countMatches}";
                }
            }
            return "Количество совпадений: 0";
        }

    }
}
