namespace Zombie.Api.Services
{
    public interface IIoService
    {
        public string CombinePath(params string[] paths);
        public Task<string> ReadAllTextAsync(string path);
    }
}
