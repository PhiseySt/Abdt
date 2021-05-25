using RepositoryBase.Interfaces;
using TaskService.Repositories.Entities;

namespace TaskService.Repositories.Interfaces
{
    public interface ITaskEfRepository : IBaseEfRepository<TaskEntity>
    {
    
    }
}
