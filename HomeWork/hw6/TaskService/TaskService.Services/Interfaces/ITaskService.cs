using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Entities.Models;

namespace TaskService.Services.Interfaces
{

    public interface ITaskService
    {
        Task<TaskModel> CreateTaskAsync(TaskModel taskModel);
        Task<TaskModel> GetTaskByIdAsync(Guid id);
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<TaskModel> GetFirstTasksAsync();
    }
}
