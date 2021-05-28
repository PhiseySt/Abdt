using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using TextService.Entities.Models;
using TextService.Repositories.Entities;
using TextService.Repositories.Interfaces;
using TextService.Services.Interfaces;
using TextService.Services.TextDapperService;
using Xunit;

namespace TextService.Test
{
    public class TextServiceControllerTest
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ITextDapperRepository> _textRepository = new Mock<ITextDapperRepository>();
        private readonly Mock<IFormFile> _fileMock = new Mock<IFormFile>();
        private readonly ITextService _textService ;
        public TextServiceControllerTest()
        {
            _textService = new TextDapperService(_textRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetByIdAsyncShouldExecuteRepositoryGetByIdAsync()
        {
            // Arrange
            var val = Guid.NewGuid();
            var textEntity = new TextEntity {Id = val };
            _textRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(textEntity);
            _mapper.Setup(m => m.Map<TextFile>(It.IsAny<TextEntity>())).Returns(new TextFile { Id = val });

            // Act
            var result = await _textService.GetByIdAsync(val);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(textEntity.Id, result.Value.Id);
            _textRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task GetAllAsyncShouldExecuteRepositoryGetAllAsync()
        {
            // Arrange
            var listTextEntities = new List<TextEntity>();
            var listTextFiles = new List<TextFile>();
            for (var i = 0; i < 5; i++)
            {
                var val = Guid.NewGuid();
                var textEntity = new TextEntity { Id = val };
                listTextEntities.Add(textEntity);
                var textFile= new TextFile { Id = val };
                listTextFiles.Add(textFile);
            }

            _textRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(listTextEntities);
            _mapper.Setup(m => m.Map<IEnumerable<TextFile>>(It.IsAny<IEnumerable<TextEntity>>())).Returns(listTextFiles);

            // Act
            var result = await _textService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listTextFiles, result.ToList());
            _textRepository.Verify(x => x.GetAllAsync());
        }

        [Fact]
        public async Task AddFileByUrlAsyncShouldExecuteRepositoryCreateAsync()
        {
            // Arrange
            var filePath = "https://filesamples.com/samples/document/txt/sample3.txt";
            var resultString = "Quod equidem non reprehendo;";
            var val = Guid.NewGuid();
            var textEntity = new TextEntity { Id = val, TextValue = resultString };
            var textFile = new TextFile() { Id = val, TextValue = resultString };
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

              _textRepository.Setup(x => x.CreateAsync(It.IsAny<TextEntity>())).ReturnsAsync(textEntity);
              _mapper.Setup(m => m.Map<TextFile>(It.IsAny<TextEntity>())).Returns(textFile);

            // Act
            var result = await _textService.AddFileByUrlAsync(filePath, token);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TextFile>(result);
            Assert.Equal(textFile.TextValue, result.TextValue);
        }

        [Fact]
        public async Task AddFileAsyncShouldExecuteRepositoryCreateAsync()
        {
            // Arrange
            var testString = "Quod equidem non reprehendo;";
            var val = Guid.NewGuid();
            var textEntity = new TextEntity { Id = val, TextValue = testString };
            var textFile = new TextFile() { Id = val, TextValue = testString };

            _textRepository.Setup(x => x.CreateAsync(It.IsAny<TextEntity>())).ReturnsAsync(textEntity);
            _mapper.Setup(m => m.Map<TextFile>(It.IsAny<TextEntity>())).Returns(textFile);

            // Act
            var result = await _textService.AddFileAsync(testString);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TextFile>(result);
            Assert.Equal(textFile.TextValue, result.TextValue);
        }

        [Fact]
        public async Task UploadFileAsyncShouldExecuteRepositoryCreateAsync()
        {
            // Arrange
            var testString = "Quod equidem non reprehendo;";
            var val = Guid.NewGuid();
            var textEntity = new TextEntity { Id = val, TextValue = testString };
            var textFile = new TextFile() { Id = val, TextValue = testString };
            var physicalFile = new FileInfo("filePath");
            var fileName = physicalFile.Name;
            var file = _fileMock.Object;

            _fileMock.Setup(_ => _.FileName).Returns(fileName);
            _textRepository.Setup(x => x.CreateAsync(It.IsAny<TextEntity>())).ReturnsAsync(textEntity);
            _mapper.Setup(m => m.Map<TextFile>(It.IsAny<TextEntity>())).Returns(textFile);

            // Act
            var result = await _textService.UploadFileAsync(file);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TextFile>(result);
            Assert.Equal(textFile.TextValue, result.TextValue);
        }

    }
}
