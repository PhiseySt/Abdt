using RepositoryBase.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Repositories.Entities
{
    [Table("TextTaskEntity")]
    public class TextTaskEntity : BaseEntity
    {
        public Guid TaskId { get; set; }
        public Guid TextId { get; set; }
        public int FindindWordsCount { get; set; }
    }
}
