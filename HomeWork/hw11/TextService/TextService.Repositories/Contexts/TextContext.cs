using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TextService.Repositories.Entities;

namespace TextService.Repositories.Contexts
{
    public class TextContext : DbContext
    {
        private readonly IOptions<TextDbOption> _textDbOptions;
        public DbSet<TextEntity> TextEntities { get; set; }

        public TextContext(IOptions<TextDbOption> textDbOptions)
        {
            _textDbOptions = textDbOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_textDbOptions.Value.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
