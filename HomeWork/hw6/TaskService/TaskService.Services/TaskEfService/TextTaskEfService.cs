using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Entities.Models;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;
using TaskService.Services.Interfaces;

namespace TaskService.Services.TaskEfService
{
    public class TextTaskEfService : ITextTaskService
    {
        private readonly ITextTaskEfRepository _textTaskEfRepository;
        private readonly IMapper _mapper;

        public TextTaskEfService(
            ITextTaskEfRepository textTaskEfRepository,
            IMapper mapper)
        {
            _textTaskEfRepository = textTaskEfRepository;
            _mapper = mapper;
        }

        public async Task<TextTaskModel> CreateTextTaskAsync(TextTaskModel textTaskModel)
        {
            var textTaskEntity = new TextTaskEntity
            {
                TaskId = textTaskModel.TaskId,
                TextId = textTaskModel.TextId,
                FindindWordsCount = textTaskModel.FindindWordsCount
            };
            textTaskEntity = await _textTaskEfRepository.CreateAsync(textTaskEntity);
            return _mapper.Map<TextTaskModel>(textTaskEntity);
        }
        public async Task<TextTaskModel> GetTextTaskByIdAsync(Guid id)
        {
            var text = await _textTaskEfRepository.GetByIdAsync(id);

            return _mapper.Map<TextTaskModel>(text);
        }
        public async Task<IEnumerable<TextTaskModel>> GetAllTextTasksAsync()
        {
            var text = await _textTaskEfRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TextTaskEntity>, IEnumerable<TextTaskModel>>(text);

        }
    }
}
