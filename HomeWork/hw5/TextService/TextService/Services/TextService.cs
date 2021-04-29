using System;
using System.Threading.Tasks;
using AutoMapper;
using Text.Service.Repositories;
using TextService.Entities;

namespace TextService.Services
{
    public class TextService : ITextService
    {
        private readonly ITextRepository _textRepository;
        private readonly IMapper _mapper;

        public TextService(ITextRepository textRepository,
        IMapper mapper)
        {
            _textRepository = textRepository;
            _mapper = mapper;
        }

        public async Task<TextFile> AddFile(string text)
        {
            var textFile = new Text.Service.Repositories.Text();
            textFile.TextValue = text;

            textFile = await _textRepository.Create(textFile);
            textFile.TextValue = null;

            return _mapper.Map<TextFile>(textFile);
        }

        public async Task<TextFile> GetById(Guid id)
        {
            var text = await _textRepository.GetById(id);
            return _mapper.Map<TextFile>(text);
        }
    }
}