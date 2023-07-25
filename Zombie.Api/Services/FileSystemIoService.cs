using Zombie.Api.Exceptions;

namespace Zombie.Api.Services
{
    public class FileSystemIoService : IIoService
    {
        public bool CheckPathIsWritable(string path)
        {
            try
            {
                string tempFileName = $"{Guid.NewGuid()}.tmp";
                string tempFileFullPath = CombinePath(path, tempFileName);
                using var outputStream = new FileStream(
                    tempFileFullPath,
                    FileMode.CreateNew,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    1024,
                    FileOptions.DeleteOnClose);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string CombinePath(params string[] paths)
        {
            var combined = Path.Combine(paths);
            combined = combined.Replace("\\", "/"); // Consistency between Windows & Linux
            return combined;
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public async Task<string> ReadAllTextAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        public async Task WriteAllTextAsync(
            string path,
            string content)
        {
            await File.WriteAllTextAsync(path, content);
        }
    }
}
