using System.Linq.Expressions;
using Zombie.Api.Dto.Models;
using Zombie.Api.Exceptions;
using Zombie.Api.Services;

namespace Zombie.Api.Repositories
{
    public class FileSystemDocumentRepository : IRepository<Document>
    {
        private readonly FileSystemRepositoryOptions _options;
        private readonly IIoService _ioService;
        private readonly IDocumentParser _documentParser;

        public FileSystemDocumentRepository(
            FileSystemRepositoryOptions options,
            IIoService ioService,
            IDocumentParser documentParser)
        {
            _options = options;
            _ioService = ioService;
            _documentParser = documentParser;
            CheckBasePathPermissions();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Document entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Document>> Get<TKey>(
            Expression<Func<Document, bool>>? filter = null,
            Func<Document, TKey>? orderBy = null)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResponse<Document>> Get(string key)
        {
            var path = _ioService.CombinePath(_options.BasePath, key);
            if (File.Exists(path))
            {
                var data = await _ioService.ReadAllTextAsync(path);
                var document = _documentParser.Parse(data);
                return new RepositoryResponse<Document>(Enums.Status.Success, document);
            }

            return new RepositoryResponse<Document>(Enums.Status.NotFound, null);
        }

        public Task<RepositoryResponse<Document>> Insert(Document entity)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResponse<Document>> Update(Document entity)
        {
            throw new NotImplementedException();
        }

        private void CheckBasePathPermissions()
        {
            if(string.IsNullOrEmpty(_options.BasePath))
            {
                throw new FileSystemRepositoryMissingBasePathOption();
            }

            try
            {
                string tempFileName = $"{Guid.NewGuid()}.tmp";
                string tempFileFullPath = Path.Combine(_options.BasePath, tempFileName);
                using var outputStream = new FileStream(
                    tempFileFullPath,
                    FileMode.CreateNew,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    1024,
                    FileOptions.DeleteOnClose);
            }
            catch(Exception)
            {
                throw new FileSystemRepositoryBasePathNotWritableException(_options.BasePath);
            }
        }
    }
}
