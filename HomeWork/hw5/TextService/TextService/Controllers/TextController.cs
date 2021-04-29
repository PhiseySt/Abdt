using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<TextFile>> GetById(Guid id)
        {
            return await _textService.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<TextFile>> Post([FromBody]string text)
        {
            //do something
            var textFile = await _textService.AddFile(text);
            return new OkObjectResult(textFile);
        }

        [HttpPost("file/{streamTextFile}")]
        public ActionResult<TextFile> PostFile(Stream streamTextFile)
        {
            //do something
            var textFile = new TextFile();
            return new OkObjectResult(textFile);
        }

        [HttpPost("url/{fileUrl}")]
        public ActionResult<TextFile> PostFileUrl(string textFileUrl)
        {
            //do something
            var textFile = new TextFile();
            return new OkObjectResult(textFile);
        }
    }
}
