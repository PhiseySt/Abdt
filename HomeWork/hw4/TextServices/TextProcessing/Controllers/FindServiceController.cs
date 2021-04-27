using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TextProcessing.StorageData;

namespace TextProcessing.Controllers
{
    /// <summary>
    /// FindServiceController
    /// </summary>
    [ApiController]
    public class FindServiceController : ControllerBase
    {
        /// <summary>
        /// Подсчет количества слов в тексте по заданному id из принимаемого массива слов
        /// </summary>
        /// <param name="id">Универсальный идентификатор текста</param>
        /// <param name="setWords">Набор слов для поиска</param>
        [Route("api/Get/TextByMask")]
        [HttpGet]
        public string GetWordsByMask([BindRequired] int id, [FromQuery][BindRequired] string[] setWords)
        {
            var text = Storage.StorageTexts.Where(item => item.Key == id).Select(d => d.Value).FirstOrDefault();
            if (id == 0 || setWords.Length == 0 || text == null) return "Количество совпадений: 0";

            var wordsFromText = text.Split(" ");
            var countMatches = setWords.Sum(word => wordsFromText.Count(wft => wft == word));

            return $"Количество совпадений: {countMatches}";

        }
    }
}
