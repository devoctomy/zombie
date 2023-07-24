namespace Zombie.Api.Services
{
    public class FileSystemIoService : IIoService
    {
        public string CombinePath(params string[] paths)
        {
            var combined = Path.Combine(paths);
            combined = combined.Replace("\\", "/"); // Consistency between Windows & Linux
            return combined;
        }

        public async Task<string> ReadAllTextAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }
    }
}
