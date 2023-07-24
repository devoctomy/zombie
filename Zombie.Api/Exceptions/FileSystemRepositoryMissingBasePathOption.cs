namespace Zombie.Api.Exceptions
{
    public class FileSystemRepositoryMissingBasePathOption : ZombieApiException
    {
        public FileSystemRepositoryMissingBasePathOption()
            : base("Base path has not been set.")
        {
        }
    }
}
