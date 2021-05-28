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
    public class TextTaskServiceControllerTest
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ITextTaskEfRepository> _textTaskRepository = new Mock<ITextTaskEfRepository>();
        private readonly ITextTaskService _textTaskService;

        public TextTaskServiceControllerTest()
        {
            _textTaskService = new TextTaskEfService(_textTaskRepository.Object, _mapper.Object);
        }
        [Fact]
        public async Task GetTextTaskByIdAsyncShouldExecuteRepositoryGetTextTaskByIdAsync()
        {
            // Arrange
            var val = Guid.NewGuid();
            var textTaskEntity = new TextTaskEntity { Id = val };
            _textTaskRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(textTaskEntity);
            _mapper.Setup(m => m.Map<TextTaskModel>(It.IsAny<TextTaskEntity>())).Returns(new TextTaskModel { Id = val });

            // Act
            var result = await _textTaskService.GetTextTaskByIdAsync(val);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsDeleted);
            _textTaskRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task GetAllTasksAsyncShouldExecuteRepositoryGetAllTasksAsync()
        {
            // Arrange
            var listTaskEntities = new List<TextTaskEntity>();
            var listTaskModels = new List<TextTaskModel>();
            for (var i = 0; i < 5; i++)
            {
                var val = Guid.NewGuid();
                var textTaskEntity = new TextTaskEntity { Id = val };
                listTaskEntities.Add(textTaskEntity);
                var textTaskModel = new TextTaskModel() { Id = val };
                listTaskModels.Add(textTaskModel);
            }
            _textTaskRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(listTaskEntities);
            _mapper.Setup(m => m.Map<IEnumerable<TextTaskModel>>(It.IsAny<IEnumerable<TextTaskEntity>>())).Returns(listTaskModels);

            // Act
            var result = await _textTaskService.GetAllTextTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, listTaskModels);
            _textTaskRepository.Verify(x => x.GetAllAsync());
        }

        [Fact]
        public async Task CreateTaskAsyncShouldExecuteRepositoryCreateAsync()
        {
            // Arrange
            var val = Guid.NewGuid();
            var textTaskEntity = new TextTaskEntity { Id = val };
            var textTaskModel = new TextTaskModel() { Id = val };
            _textTaskRepository.Setup(x => x.CreateAsync(It.IsAny<TextTaskEntity>())).ReturnsAsync(textTaskEntity);
            _mapper.Setup(m => m.Map<TextTaskModel>(It.IsAny<TextTaskEntity>())).Returns(new TextTaskModel { Id = val });

            // Act
            var result = await _textTaskService.CreateTextTaskAsync(textTaskModel);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsDeleted);
            Assert.Equal(textTaskModel.Id, result.Id);
        }
    }
}
