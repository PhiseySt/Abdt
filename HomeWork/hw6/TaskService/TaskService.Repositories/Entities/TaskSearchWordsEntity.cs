using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Repositories.Entities
{
    [Table("TaskSearchWordsEntity")]
    public class TaskSearchWordsEntity
    {
        public Guid Id { get; set; } = new Guid();
        public string FindWord { get; set; }
        public TaskEntity TaskEntity { get; set; }
    }
}
