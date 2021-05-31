using RepositoryBase.Interfaces;
using TaskService.Repositories.Entities;

namespace TaskService.Repositories.Interfaces
{
    public interface ITaskDapperRepository : IBaseDapperRepository<TaskEntity>
    {
    }
}
