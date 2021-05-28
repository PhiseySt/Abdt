using RepositoryBase.Entities;
using System;

namespace TaskService.Entities.Models
{
    public class TextTaskModel 
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public Guid TaskId { get; set; }
        public Guid TextId { get; set; }
        public int FindindWordsCount { get; set; }
    }
}
