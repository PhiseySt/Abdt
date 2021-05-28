using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindService.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using TextService.Client;
using TextService.Entities.Models;
using Xunit;

namespace FindService.Test
{
    public class FindServiceControllerTest
    {
        private readonly Mock<ITextClient> _textClient = new Mock<ITextClient>();
        private readonly Mock<ILogger<FindServiceController>> _logger = new Mock<ILogger<FindServiceController>>();
        private readonly FindServiceController _controller;
        private readonly IEnumerable<TextFile> _allTexts;
        private readonly string[] _setWords = { "someWord", "wordExist" };
        private readonly string[] _setWords2 = { "someWord2", "wordExist2" };
        public FindServiceControllerTest()
        {
            _controller = new FindServiceController(_textClient.Object, _logger.Object);
            var val1 = Guid.NewGuid();
            var textFile1 = new TextFile { Id = val1, TextValue = "someWord" };
            var val2 = Guid.NewGuid();
            var textFile2 = new TextFile { Id = val2, TextValue = "wordExist" };
            _allTexts = new List<TextFile> { textFile1, textFile2 };
        }

        [Fact]
        public async Task IsExistWordAsyncShouldGetYesIfWordExist()
        {
            // Arrange
            var testWord = "wordExist";
            _textClient.Setup(x => x.GetAllTexts()).ReturnsAsync(_allTexts);

            // Act
            var result = await _controller.IsExistWordAsync(testWord);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal("Слово найдено при поиске", result);
        }

        [Fact]
        public async Task IsExistWordAsyncShouldGetNoIfWordNotExist()
        {
            // Arrange
            var testWord = "wordNotExist";
            _textClient.Setup(x => x.GetAllTexts()).ReturnsAsync(_allTexts);

            // Act
            var result = await _controller.IsExistWordAsync(testWord);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal("Слово не найдено при поиске", result);
        }

        [Fact]
        public async Task GetWordsByMaskAsyncShouldGetCountWord1()
        {
            // Arrange
            var val1 = Guid.NewGuid();
            _textClient.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(_allTexts.First());

            // Act
            var result = await _controller.GetWordsByMaskAsync(val1, _setWords);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal("Количество совпадений: 1", result);
        }

        [Fact]
        public async Task GetWordsByMaskAsyncShouldGetCountWord0()
        {
            // Arrange
            var val1 = Guid.NewGuid();
            _textClient.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(_allTexts.First);

            // Act
            var result = await _controller.GetWordsByMaskAsync(val1, _setWords2);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal("Количество совпадений: 0", result);
        }
    }
}
