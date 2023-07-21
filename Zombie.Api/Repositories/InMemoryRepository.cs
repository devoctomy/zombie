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

        public RepositoryResponse<T> Get(string id)
        {
            _entities.TryGetValue(id, out var entity);
            return new RepositoryResponse<T>(entity != null ? Enums.Status.Success : Enums.Status.NotFound, entity);
        }

        public RepositoryResponse<T> InsertNew(T entity)
        {
            var id = Guid.NewGuid().ToString();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            if (_entities.TryAdd(
                id,
                entity))
            {
                entity.Id = id;
                return new RepositoryResponse<T>(Enums.Status.Success, entity);
            }

            return new RepositoryResponse<T>(Enums.Status.Fail, null);
        }

        public RepositoryResponse<T> Update(T entity)
        {
            if(string.IsNullOrEmpty(entity.Id))
            {
                throw new ArgumentException("Entity does not have an Id");
            }

            var existing = Get(entity.Id);
            if(existing == null || existing.Value == null)
            {
                return new RepositoryResponse<T>(Enums.Status.NotFound, null);
            }

            existing.Value.UpdatedAt = DateTime.UtcNow;
            if (_entities.TryUpdate(
                entity.Id,
                entity,
                existing.Value))
            {
                return new RepositoryResponse<T>(Enums.Status.Success, entity);
            }

            return new RepositoryResponse<T>(Enums.Status.Fail, null);
        }
    }
}
