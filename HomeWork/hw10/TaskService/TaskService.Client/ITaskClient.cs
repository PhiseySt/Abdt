using Refit;
using System;
using System.Threading.Tasks;
using TaskService.Entities.Models;

namespace TaskService.Client
{
    public interface ITaskClient
    {
        [Get("/taskservice/{id}")]
        Task<TaskModel> GetTaskById(Guid id);

        [Get("/taskservice")]
        Task<TaskModel> GetAllTask();

        [Post("/taskservice/task")]
        Task<TaskModel> Post(TaskModel taskModel);


        [Get("/texttaskservice/{id}")]
        Task<TextTaskModel> GetTextTaskById(Guid id);
       
        [Get("/texttaskservice")]
        Task<TextTaskModel> GetAllTextTask();
       
        [Post("/texttaskservice/texttask")]
        Task<TextTaskModel> Post(TextTaskModel textTaskModel);
    }
}
