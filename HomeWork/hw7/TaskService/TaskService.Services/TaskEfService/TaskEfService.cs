using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.Entities.Models;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;
using TaskService.Services.Interfaces;

namespace TaskService.Services.TaskEfService
{
    public class TaskEfService : ITaskService
    {
        private readonly ITaskEfRepository _taskEfRepository;
        private readonly IMapper _mapper;

        public TaskEfService(
            ITaskEfRepository taskEfRepository,
            IMapper mapper)
        {
            _taskEfRepository = taskEfRepository;
            _mapper = mapper;
        }

        public async Task<TaskModel> CreateTaskAsync(TaskModel taskModel)
        {
            var taskEntitys = _mapper.Map<IEnumerable<TaskSearchWordsEntity>>(taskModel.TaskFindWordsModels);
            var taskEntity = new TaskEntity
            {
                TaskStartTime = taskModel.TaskStartTime,
                TaskEndTime = taskModel.TaskEndTime,
                TaskInterval = taskModel.TaskInterval,
                TaskSearchWordsEntities = taskEntitys
            };
            taskEntity = await _taskEfRepository.CreateAsync(taskEntity);

            var findModels = _mapper.Map<IEnumerable<TaskFindWordsModel>>(taskEntity.TaskSearchWordsEntities);
            var taskModelresult = _mapper.Map<TaskModel>(taskEntity);
            taskModelresult.TaskFindWordsModels = findModels;

            return taskModelresult;
        }
        public async Task<TaskModel> GetTaskByIdAsync(Guid id)
        {
            var task = await _taskEfRepository.GetByIdAsync(id);

            return _mapper.Map<TaskModel>(task);
        }
        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            var task = await _taskEfRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(task);

        }
        public async Task<TaskModel> GetFirstTasksAsync()
        {
            var tasks = await _taskEfRepository.GetAllAsync();

            var task = tasks.OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            return _mapper.Map<TaskEntity, TaskModel>(task);

        }
    }
}
