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

        public async Task<RepositoryResponse<Document>> Delete(string key)
        {
            var existingDocument = await Get(key);
            if(existingDocument.Status != Enums.Status.Success)
            {
                return existingDocument;
            }

            var path = _ioService.CombinePath(_options.BasePath, key);
            _ioService.Delete(path);
            return existingDocument;
        }

        public async Task<RepositoryResponse<Document>> Delete(Document entity)
        {
            if (string.IsNullOrEmpty(entity.Key))
            {
                throw new ArgumentException("Entity does not have a Key");
            }

            return await Delete(entity.Key);
        }

        public Task<IEnumerable<Document>> Get<TKey>(
            Expression<Func<Document, bool>>? filter = null,
            Func<Document, TKey>? orderBy = null)
        {
            throw new NotImplementedException(); // Not sure I can do this with the file system
        }

        public async Task<RepositoryResponse<Document>> Get(string key)
        {
            var path = _ioService.CombinePath(_options.BasePath, key);
            if (_ioService.Exists(path))
            {
                var data = await _ioService.ReadAllTextAsync(path);
                var document = _documentParser.Parse(data);
                return new RepositoryResponse<Document>(Enums.Status.Success, document);
            }

            return new RepositoryResponse<Document>(Enums.Status.NotFound, null);
        }

        public async Task<RepositoryResponse<Document>> Insert(Document entity)
        {
            var path = _ioService.CombinePath(_options.BasePath, entity.Key);
            if (!_ioService.Exists(path))
            {
                var document = _documentParser.Serialise(entity);
                await _ioService.WriteAllTextAsync(path, document);
                return new RepositoryResponse<Document>(Enums.Status.Success, entity);
            }

            return new RepositoryResponse<Document>(Enums.Status.Conflict, null);
        }

        public async Task<RepositoryResponse<Document>> Update(Document entity)
        {
            var path = _ioService.CombinePath(_options.BasePath, entity.Key);
            if (_ioService.Exists(path))
            {
                var document = _documentParser.Serialise(entity);
                await _ioService.WriteAllTextAsync(path, document);
                return new RepositoryResponse<Document>(Enums.Status.Success, entity);
            }

            return new RepositoryResponse<Document>(Enums.Status.NotFound, null);
        }

        private void CheckBasePathPermissions()
        {
            if(string.IsNullOrEmpty(_options.BasePath))
            {
                throw new FileSystemRepositoryMissingBasePathOption();
            }

            if(!_ioService.CheckPathIsWritable(_options.BasePath))
            {
                throw new FileSystemRepositoryBasePathNotWritableException(_options.BasePath);
            }
        }
    }
}
