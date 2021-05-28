using Microsoft.Extensions.Options;
using RepositoryBase.Repositories;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;

namespace TaskService.Repositories.Repositories
{
    public class TaskDapperRepository : BaseDapperRepository<TaskEntity>, ITaskDapperRepository
    {
        public TaskDapperRepository(IOptions<TaskDbOption> dbOption) : base(dbOption)
        {
        }
    }
}
