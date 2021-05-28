using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Entities.Models;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;
using TaskService.Services.Interfaces;

namespace TaskService.Services.TaskDapperService
{
    public class TextTaskDapperService : ITextTaskService
    {
        private readonly ITextTaskDapperRepository _textTaskDapperRepository;
        private readonly IMapper _mapper;

        public TextTaskDapperService(
            ITextTaskDapperRepository textTaskDapperRepository,
            IMapper mapper)
        {
            _textTaskDapperRepository = textTaskDapperRepository;
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
            textTaskEntity = await _textTaskDapperRepository.CreateAsync(textTaskEntity);
            return _mapper.Map<TextTaskModel>(textTaskEntity);
        }
        public async Task<TextTaskModel> GetTextTaskByIdAsync(Guid id)
        {
            var text = await _textTaskDapperRepository.GetByIdAsync(id);

            return _mapper.Map<TextTaskModel>(text);
        }
        public async Task<IEnumerable<TextTaskModel>> GetAllTextTasksAsync()
        {
            var text = await _textTaskDapperRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TextTaskEntity>, IEnumerable<TextTaskModel>>(text);

        }
    }
}
