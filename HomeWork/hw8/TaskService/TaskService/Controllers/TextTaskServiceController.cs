using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Entities.Models;
using TaskService.Services.Interfaces;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextTaskServiceController : ControllerBase
    {
        private readonly ITextTaskService _taskService;
        private readonly ILogger<TextTaskServiceController> _logger;

        public TextTaskServiceController(ITextTaskService taskService, ILogger<TextTaskServiceController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet("{id}")]
         public async Task<ActionResult<TextTaskModel>> GetTextTaskById(Guid id)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetTextTaskById {bearerToken} {DateTime.Now}");
            var result = await _taskService.GetTextTaskByIdAsync(id);
            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<TextTaskModel>> GetAllTextTask()
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetAllTextTask {bearerToken} {DateTime.Now}");
            var result = await _taskService.GetAllTextTasksAsync();
            return result;
        }

        [HttpPost("texttask")]
        public async Task<ActionResult<TaskModel>> PostTextTask([FromForm] TextTaskModel textTaskModel)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"PostTextTask {bearerToken} {DateTime.Now}");
            var textFile = await _taskService.CreateTextTaskAsync(textTaskModel);
            return new OkObjectResult(textFile);
        }
    }
}
