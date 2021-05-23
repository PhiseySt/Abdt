using System;

namespace TaskService.Entities.Models
{
    public class TaskFindWordsModel
    {
        public Guid Id { get; set; }
        public string FindWord { get; set; }
        public TaskModel TaskModel { get; set; }
    }
}
