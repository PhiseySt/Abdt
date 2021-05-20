using Microsoft.Extensions.Options;
using RepositoryBase.Repositories;
using TaskService.Repositories.Entities;
using TaskService.Repositories.Interfaces;

namespace TaskService.Repositories.Repositories
{
    public class TextTaskDapperRepository : BaseDapperRepository<TextTaskEntity>, ITextTaskDapperRepository
    {
        public TextTaskDapperRepository(IOptions<TaskDbOption> dbOption) : base(dbOption)
        {
        }
    }
}
