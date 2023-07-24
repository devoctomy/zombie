namespace Zombie.Api.Exceptions
{
    public class FileSystemRepositoryBasePathNotWritableException : ZombieApiException
    {
        public FileSystemRepositoryBasePathNotWritableException(string basePath)
            : base($"The base path '{basePath}' is not writable.")
        {
        }
    }
}
