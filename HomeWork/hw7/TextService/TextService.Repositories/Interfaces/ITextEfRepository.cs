using RepositoryBase.Interfaces;
using TextService.Repositories.Entities;

namespace TextService.Repositories.Interfaces
{
    public interface ITextEfRepository : IBaseEfRepository<TextEntity>
    {
    }
}
