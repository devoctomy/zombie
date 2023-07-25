namespace Zombie.Api.Services
{
    public interface IIoService
    {
        public string CombinePath(params string[] paths);
        public bool Exists(string path);
        public void Delete(string path);
        public Task<string> ReadAllTextAsync(string path);
        public Task WriteAllTextAsync(string path, string content);
        public bool CheckPathIsWritable(string path);
    }
}
