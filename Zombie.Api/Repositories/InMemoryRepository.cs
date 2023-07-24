using System.Collections.Concurrent;
using System.Linq.Expressions;
using Zombie.Api.Dto.Models;

namespace Zombie.Api.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : Entity
    {
        private ConcurrentDictionary<string, T> _entities = new();

        public Task<IEnumerable<T>> Get<TKey>(
            Expression<Func<T, bool>>? filter = null,
            Func<T, TKey>? orderBy = null)
        {
            var queryable = _entities.Values.AsQueryable();
            if(filter != null)
            {
                queryable = queryable.Where(filter);
            }

            var enumerable = queryable.AsEnumerable();
            if (orderBy != null)
            {
                enumerable = enumerable.OrderBy(orderBy);
            }

            return Task.FromResult(enumerable);
        }

        public Task<bool> Delete(string id)
        {
            return Task.FromResult(_entities.TryRemove(
                id,
                out var _));
        }

        public Task<bool> Delete(T entity)
        {
            if (string.IsNullOrEmpty(entity.Key))
            {
                throw new ArgumentException("Entity does not have a Key");
            }

            return Delete(entity.Key);
        }

        public Task<RepositoryResponse<T>> Get(string key)
        {
            _entities.TryGetValue(key, out var entity);
            return Task.FromResult(new RepositoryResponse<T>(entity != null ? Enums.Status.Success : Enums.Status.NotFound, entity));
        }

        public Task<RepositoryResponse<T>> Insert(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            if (_entities.TryAdd(
                entity.Key,
                entity))
            {
                return Task.FromResult(new RepositoryResponse<T>(Enums.Status.Success, entity));
            }

            // !!! Does this get hit if it already exists?
            return Task.FromResult(new RepositoryResponse<T>(Enums.Status.Fail, null));
        }

        public async Task<RepositoryResponse<T>> Update(T entity)
        {
            if(string.IsNullOrEmpty(entity.Key))
            {
                throw new ArgumentException("Entity does not have a Key");
            }

            var existing = await Get(entity.Key);
            if(existing == null || existing.Value == null)
            {
                return new RepositoryResponse<T>(Enums.Status.NotFound, null);
            }

            existing.Value.UpdatedAt = DateTime.UtcNow;
            if (_entities.TryUpdate(
                entity.Key,
                entity,
                existing.Value))
            {
                return new RepositoryResponse<T>(Enums.Status.Success, entity);
            }

            return new RepositoryResponse<T>(Enums.Status.Fail, null);
        }
    }
}
