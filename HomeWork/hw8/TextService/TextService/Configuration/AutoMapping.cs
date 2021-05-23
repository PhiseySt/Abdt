using AutoMapper;
using System.Collections.Generic;
using TextService.Repositories.Entities;
using TextService.Entities.Models;

namespace TextService.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<TextEntity, TextFile>().ReverseMap();
        }
    }
}
