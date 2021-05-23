using Microsoft.Extensions.Options;
using RepositoryBase.Repositories;
using TextService.Repositories.Contexts;
using TextService.Repositories.Entities;
using TextService.Repositories.Interfaces;

namespace TextService.Repositories.Repositories
{
    public class TextEfRepository : BaseEfRepository<TextEntity>, ITextEfRepository
    {
        public TextEfRepository(IOptions<TextDbOption> dbOption, TextContext textContext) : base(dbOption, textContext)
        {
        }
    }
}
