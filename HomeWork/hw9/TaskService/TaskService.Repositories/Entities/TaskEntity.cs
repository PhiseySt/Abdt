using RepositoryBase.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Repositories.Entities
{
    [Table("TaskEntity")]
    public class TaskEntity : BaseEntity
    {
        public int TaskInterval { get; set; } = 20;
        public DateTime TaskStartTime { get; set; } = DateTime.Now;
        public DateTime TaskEndTime { get; set; } = DateTime.Now.AddHours(2);
        public IEnumerable<TaskSearchWordsEntity>  TaskSearchWordsEntities { get; set; }
    }
}
