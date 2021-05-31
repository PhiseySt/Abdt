using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskService.Repositories.Entities;

namespace TaskService.Repositories.Contexts
{
    public class TaskContext : DbContext
    {
        private readonly IOptions<TaskDbOption> _taskDbOptions;
        public DbSet<TaskSearchWordsEntity> TaskSearchWordsEntities { get; set; }
        public DbSet<TaskEntity> TaskEntities { get; set; }
        public DbSet<TextTaskEntity> TextTaskEntities { get; set; }

        public TaskContext(IOptions<TaskDbOption> taskDbOptions)
        {
            _taskDbOptions = taskDbOptions;
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_taskDbOptions.Value.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
