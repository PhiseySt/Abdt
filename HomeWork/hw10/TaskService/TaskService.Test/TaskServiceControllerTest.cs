using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskService.Entities.Models;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;
using TaskService.Services.Interfaces;
using TaskService.Services.TaskEfService;
using Xunit;

namespace TaskService.Test
{
    public class TaskServiceControllerTest
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ITaskEfRepository> _taskRepository = new Mock<ITaskEfRepository>();
        private readonly ITaskService _taskService;

        public TaskServiceControllerTest()
        {
            _taskService = new TaskEfService(_taskRepository.Object, _mapper.Object);
        }
        [Fact]
        public async Task GetTaskByIdAsyncShouldExecuteRepositoryGetTaskByIdAsync()
        {
            // Arrange
            var val = Guid.NewGuid();
            var taskEntity = new TaskEntity{ Id = val };
               _taskRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(taskEntity);
               _mapper.Setup(m => m.Map<TaskModel>(It.IsAny<TaskEntity>())).Returns(new TaskModel { Id = val });

            // Act
            var result = await _taskService.GetTaskByIdAsync(val);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsDeleted);
            _taskRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task GetAllTasksAsyncShouldExecuteRepositoryGetAllTasksAsync()
        {
            // Arrange
            var listTaskEntities = new List<TaskEntity>();
            var listTaskModels = new List<TaskModel>();
            for (var i = 0; i < 5; i++)
            {
                var val = Guid.NewGuid();
                var taskEntity = new TaskEntity { Id = val };
                listTaskEntities.Add(taskEntity);
                var taskModel = new TaskModel { Id = val };
                listTaskModels.Add(taskModel);
            }
            _taskRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(listTaskEntities);
            _mapper.Setup(m => m.Map<IEnumerable<TaskModel>>(It.IsAny<IEnumerable<TaskEntity>>())).Returns(listTaskModels);

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, listTaskModels);
            _taskRepository.Verify(x => x.GetAllAsync());
        }

        [Fact]
        public async Task CreateTaskAsyncShouldExecuteRepositoryCreateAsync()
        {
            // Arrange
            var val = Guid.NewGuid();
            var taskEntity = new TaskEntity { Id = val };
            var taskModel = new TaskModel() { Id = val };
            _taskRepository.Setup(x => x.CreateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(taskEntity);
            _mapper.Setup(m => m.Map<TaskModel>(It.IsAny<TaskEntity>())).Returns(new TaskModel { Id = val });

            // Act
            var result = await _taskService.CreateTaskAsync(taskModel);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsDeleted);
            Assert.Equal(10, result.TaskInterval);
            Assert.Equal(taskModel.Id, result.Id);
        }
    }
}
