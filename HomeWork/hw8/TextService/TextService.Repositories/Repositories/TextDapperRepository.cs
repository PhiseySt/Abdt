using Microsoft.Extensions.Options;
using RepositoryBase.Repositories;
using TextService.Repositories.Entities;
using TextService.Repositories.Interfaces;

namespace TextService.Repositories.Repositories
{
    public class TextDapperRepository : BaseDapperRepository<TextEntity>, ITextDapperRepository
    {
        public TextDapperRepository(IOptions<TextDbOption> dbOption) : base(dbOption)
        {
        }
    }
}
