using System.Linq.Expressions;
using Zombie.Api.Dto.Models;

namespace Zombie.Api.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        public Task<IEnumerable<T>> Get<TKey>(
            Expression<Func<T, bool>>? filter = null,
            Func<T, TKey>? orderBy = null);

        public Task<RepositoryResponse<T>> Get(string key);

        public Task<RepositoryResponse<T>> Insert(T entity);

        public Task<bool> Delete(string key);

        public Task<bool> Delete(T entity);

        public Task<RepositoryResponse<T>> Update(T entity);
    }
}
