using System.Collections.Concurrent;
using System.Linq.Expressions;
using Zombie.Api.Dto.Models;

namespace Zombie.Api.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : Entity
    {
        private ConcurrentDictionary<string, T> _entities = new();

        public IEnumerable<T> Get<TKey>(
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

            return enumerable;
        }

        public bool Delete(string id)
        {
            return _entities.TryRemove(
                id,
                out var _);
        }

        public bool Delete(T entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new ArgumentException("Entity does not have an Id");
            }

            return Delete(entity.Id);
        }

        public T? Get(string id)
        {
            _entities.TryGetValue(id, out var entity);
            return entity;
        }

        public T? InsertNew(T entity)
        {
            var id = Guid.NewGuid().ToString();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            if (_entities.TryAdd(
                id,
                entity))
            {
                entity.Id = id;
                return entity;
            }

            return null;
        }

        public T? Update(T entity)
        {
            if(string.IsNullOrEmpty(entity.Id))
            {
                throw new ArgumentException("Entity does not have an Id");
            }

            var existing = Get(entity.Id);
            if(existing == null)
            {
                return null;
            }

            existing.UpdatedAt = DateTime.UtcNow;
            if (_entities.TryUpdate(
                entity.Id,
                entity,
                existing))
            {
                return existing;
            }

            return null;
        }
    }
}
