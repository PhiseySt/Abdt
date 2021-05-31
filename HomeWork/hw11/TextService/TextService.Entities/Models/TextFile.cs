using RepositoryBase.Entities;
using System;

namespace TextService.Entities.Models
{
    public class TextFile
    {
        public Guid Id { get; set; }
        public string TextValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
