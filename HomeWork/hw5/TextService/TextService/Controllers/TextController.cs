using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TextService.Entities;
using TextService.Services;

namespace TextService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextController : ControllerBase
    {
        private readonly ITextService _textService;
        private readonly ILogger<TextController> _logger;

        public TextController(ITextService textService, ILogger<TextController> logger)
        {
            _textService = textService;
            _logger = logger;
        }

        [HttpGet ("GetAll")]
        public async Task<IEnumerable<TextFile>> GetAll()
        {
            return await _textService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TextFile>> GetById(Guid id)
        {
            return await _textService.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<TextFile>> Post([FromBody]string text)
        {
            var textFile = await _textService.AddFile(text);
            return new OkObjectResult(textFile);
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<TextFile>> PostFile(IFormFile file)
        {
            //do something
            var textFile = await _textService.UploadFile(file);
            return new OkObjectResult(textFile);
        }

        /// <summary>
        /// Загрузить текстовый файл по url ссылке в локальное хранилище
        /// Пример текстового файла: https://filesamples.com/samples/document/txt/sample3.txt
        /// </summary>
        /// <param name="textFileUrl">Ccылка на url для загрузки файла в хранилище</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        [HttpPost("Url")]
        public async Task<ActionResult<TextFile>> PostFileUrl([FromBody] string textFileUrl, CancellationToken cancellationToken)
        {
            var textFile = await _textService.AddFileByUrl(textFileUrl, cancellationToken);
            return new OkObjectResult(textFile);
        }
    }
}
