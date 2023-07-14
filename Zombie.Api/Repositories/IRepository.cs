using System.Linq.Expressions;

namespace Zombie.Api.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        public IEnumerable<T> Get<TKey>(
            Expression<Func<T, bool>>? filter = null,
            Func<T, TKey>? orderBy = null);

        public T? Get(string id);

        public T? InsertNew(T entity);

        public bool Delete(string id);

        public bool Delete(T entity);

        public bool Update(T entity);
    }
}
