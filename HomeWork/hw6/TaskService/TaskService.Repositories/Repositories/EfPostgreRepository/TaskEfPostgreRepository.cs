using Microsoft.Extensions.Options;
using RepositoryBase.Repositories;
using TaskService.Repositories.Contexts;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;

namespace TaskService.Repositories.Repositories.EfPostgreRepository
{
    public class TaskEfPostgreRepository : BaseEfRepository<TaskEntity>, ITaskEfRepository
    {
        public TaskEfPostgreRepository(IOptions<TaskDbOption> dbOption, TaskPostgreContext context) : base(dbOption, context)
        {
        }
    }

}
