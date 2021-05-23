using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextService.Services.Interfaces;
using TextService.Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace TextService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextServiceController : ControllerBase
    {
        private readonly ITextService _textService;
        private readonly ILogger<TextServiceController> _logger;

        public TextServiceController(ITextService textService, ILogger<TextServiceController> logger)
        {
            _textService = textService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IEnumerable<TextFile>> GetAll()
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetAll {bearerToken} {DateTime.Now}");
            return await _textService.GetAllAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TextFile>> GetById(Guid id)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetById: {bearerToken} {DateTime.Now}");
            return await _textService.GetByIdAsync(id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TextFile>> Post([FromBody] string text)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"Post: {bearerToken} {DateTime.Now}");
            var textFile = await _textService.AddFileAsync(text);
            return new OkObjectResult(textFile);
        }

        [Authorize]
        [HttpPost("Upload")]
        public async Task<ActionResult<TextFile>> PostFile(IFormFile file)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"PostFile: {bearerToken} {DateTime.Now}");
            var textFile = await _textService.UploadFileAsync(file);
            return new OkObjectResult(textFile);
        }

        /// <summary>
        /// Загрузить текстовый файл по url ссылке в локальное хранилище
        /// Пример текстового файла: https://filesamples.com/samples/document/txt/sample3.txt
        /// </summary>
        /// <param name="textFileUrl">Ccылка на url для загрузки файла в хранилище</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        [Authorize]
        [HttpPost("Url")]
        public async Task<ActionResult<TextFile>> PostFileUrl([FromBody] string textFileUrl, CancellationToken cancellationToken)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"PostFileUrl: {bearerToken} {DateTime.Now}");
            var textFile = await _textService.AddFileByUrlAsync(textFileUrl, cancellationToken);
            return new OkObjectResult(textFile);
        }
    }
}
