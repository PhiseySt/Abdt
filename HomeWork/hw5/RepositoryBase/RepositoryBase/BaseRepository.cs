using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;

namespace RepositoryBase
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : EntityBase
    {
        private readonly string _connectionString;
        private static readonly PropertyInfo[] WritableEntityFields = typeof(T).GetProperties()
            .Where(x => x.CanWrite).ToArray();

        private readonly string[] _skipOnUpdateFields = {
            nameof(EntityBase.Id),
            nameof(EntityBase.CreatedDate),
            nameof(EntityBase.CreatedBy),
            nameof(EntityBase.IsDeleted)};

        public BaseRepository(IOptions<DbOption> dbOption)
        {
            _connectionString = dbOption.Value.ConnectionString;
        }

        public async Task<T> GetById(Guid id)
        {
            await using var db = await GetSqlConnection();

            return await db.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM [{typeof(T).Name}] WHERE [Id] = @id AND [IsDeleted] = 0", new { id });
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            await using var db = await GetSqlConnection();

            return await db.QueryAsync<T>(
                $"SELECT * FROM [{typeof(T).Name}] WHERE [IsDeleted] = 0");
        }

        public async Task<T> Create(T entity)
        {
            await using var db = await GetSqlConnection();

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.LastSavedDate = DateTime.UtcNow;

            var fields = string.Join(", ", typeof(T).GetProperties().Select(property => $"[{property.Name}]"));
            var values = string.Join(", ", typeof(T).GetProperties().Select(property => $"@{property.Name}"));

            await db.ExecuteAsync($"INSERT INTO {typeof(T).Name} ({fields}) VALUES ({values})", entity);

            return await GetById(entity.Id);
        }

        public async Task<IEnumerable<T>> CreateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                entity.CreatedDate = DateTime.UtcNow;
                entity.LastSavedDate = DateTime.UtcNow;
            }

            var fields = string.Join(", ", WritableEntityFields.Select(property => $"[{property.Name}]"));
            var values = string.Join(", ", WritableEntityFields.Select(property => $"@{property.Name}"));

            await db.ExecuteAsync($"INSERT INTO {typeof(T).Name} ({fields}) VALUES ({values})", entities);

            return entities;
        }

        public async Task<bool> Update(T entity)
        {
            await using var db = await GetSqlConnection();

            entity.LastSavedDate = DateTime.UtcNow;

            var notUpdatedFields = new[] { "Id", "CreatedDate", "CreatedBy", "IsDeleted" };
            var parameters = string.Join(", ",
                typeof(T).GetProperties().Where(property => !notUpdatedFields.Contains(property.Name))
                    .Select(property => $"{property.Name} = @{property.Name}"));
            var result = await db.ExecuteAsync($"UPDATE {typeof(T).Name} SET {parameters} WHERE [Id] = @Id", entity);

            return result > 0;
        }

        public async Task<bool> UpdateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                entity.LastSavedDate = DateTime.UtcNow;
            }

            var updateableFields = WritableEntityFields.Where(property => !_skipOnUpdateFields.Contains(property.Name));

            var parameters = string.Join(", ", updateableFields.Select(property => $"[{property.Name}] = @{property.Name}"));
            return await db.ExecuteAsync($"UPDATE {typeof(T).Name} SET {parameters} WHERE [Id] = @Id", entities) == entities.Count();
        }

        public async Task<bool> Delete(Guid id)
        {
            await using var db = await GetSqlConnection();
            var result = await db.ExecuteAsync($"UPDATE {typeof(T).Name} SET [IsDeleted] = 1, [LastSavedBy]=@CurrentUser, [LastSavedDate]=@DateOfDeletion WHERE [Id] = @Id",
                new { DateOfDeletion = DateTime.UtcNow, id });

            return result > 0;
        }

        public async Task<bool> DeleteMany(IEnumerable<Guid> entityIds)
        {
            var ids = string.Join(", ", entityIds.Select(id => $"'{id}'"));
            if (string.IsNullOrEmpty(ids))
            {
                return true;
            }

            await using var db = await GetSqlConnection();

            var result = await db.ExecuteAsync($"UPDATE {typeof(T).Name} SET [IsDeleted] = 1, [LastSavedDate]=@DateOfDeletion WHERE [Id] in ({ids})",
                new { DateOfDeletion = DateTime.UtcNow });

            return result > 0;
        }

        public async Task<bool> Restore(Guid id)
        {
            await using var db = await GetSqlConnection();
            var result = await db.ExecuteAsync($"UPDATE {typeof(T).Name} SET [IsDeleted] = 0, [LastSavedBy]=@CurrentUser, [LastSavedDate]=@DateOfDeletion WHERE [Id] = @id",
                new { DateOfDeletion = DateTime.UtcNow, id });

            return result > 0;
        }

        public async Task<bool> RestoreMany(IEnumerable<Guid> entityIds)
        {
            var ids = string.Join(", ", entityIds.Select(id => $"'{id}'"));
            if (string.IsNullOrEmpty(ids))
            {
                return true;
            }

            await using var db = await GetSqlConnection();

            var result = await db.ExecuteAsync($"UPDATE {typeof(T).Name} SET [IsDeleted] = 0, [LastSavedBy]=@CurrentUser, [LastSavedDate]=@DateOfRestoring WHERE [IsDeleted] = 1 AND [Id] in ({ids})",
                new { DateOfRestoring = DateTime.UtcNow });

            return result > 0;
        }

        protected async Task<SqlConnection> GetSqlConnection()
        {
            var db = new SqlConnection(_connectionString);
            try
            {
                await db.OpenAsync();
            }
            catch (Exception)
            {
                db.Dispose();
                throw;
            }

            return db;
        }
    }
}