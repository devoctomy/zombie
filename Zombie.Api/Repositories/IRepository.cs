using System.Linq.Expressions;
using Zombie.Api.Dto.Models;

namespace Zombie.Api.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        public IEnumerable<T> Get<TKey>(
            Expression<Func<T, bool>>? filter = null,
            Func<T, TKey>? orderBy = null);

        public RepositoryResponse<T> Get(string key);

        public RepositoryResponse<T> Insert(T entity);

        public bool Delete(string key);

        public bool Delete(T entity);

        public RepositoryResponse<T> Update(T entity);
    }
}
