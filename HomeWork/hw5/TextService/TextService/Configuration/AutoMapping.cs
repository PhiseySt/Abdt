using AutoMapper;
using TextService.Entities;

namespace TextService.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Text.Service.Repositories.Text, TextFile>().ReverseMap();
        }
    }
}