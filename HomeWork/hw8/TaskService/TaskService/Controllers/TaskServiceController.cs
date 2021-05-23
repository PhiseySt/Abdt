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
    public class TaskServiceController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TextTaskServiceController> _logger;

        public TaskServiceController(ITaskService taskService, ILogger<TextTaskServiceController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTaskById(Guid id)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetTaskById {bearerToken} {DateTime.Now}");
            var result = await _taskService.GetTaskByIdAsync(id);
            return result;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<TaskModel>> GetAllTask()
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"GetAllTask {bearerToken} {DateTime.Now}");
            var result = await _taskService.GetAllTasksAsync();
            return result;
        }

        [Authorize]
        [HttpPost("task")]
        public async Task<ActionResult<TaskModel>> PostTask([FromBody] TaskModel taskModel)
        {
            var bearerToken = Request.Headers["Authorization"];
            _logger.LogInformation($"PostTask {bearerToken} {DateTime.Now}");
            var textFile = await _taskService.CreateTaskAsync(taskModel);
            return new OkObjectResult(textFile);
        }

    }
}
