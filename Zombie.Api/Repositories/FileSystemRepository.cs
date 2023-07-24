using System.Linq.Expressions;
using Zombie.Api.Dto.Models;

namespace Zombie.Api.Repositories
{
    public class FileSystemRepository<T> : IRepository<T> where T : Entity
    {
        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get<TKey>(
            Expression<Func<T, bool>>? filter = null,
            Func<T, TKey>? orderBy = null)
        {
            throw new NotImplementedException();
        }

        public RepositoryResponse<T> Get(string id)
        {
            throw new NotImplementedException();
        }

        public RepositoryResponse<T> Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public RepositoryResponse<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
