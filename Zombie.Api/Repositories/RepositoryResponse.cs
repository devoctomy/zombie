using Zombie.Api.Repositories.Enums;

namespace Zombie.Api.Repositories
{
    public class RepositoryResponse<T>
    {
        public Status Status { get; }
        public T? Value { get; }

        public RepositoryResponse(
            Status status,
            T? value)
        {
            Status = status;
            Value = value;
        }
    }
}
