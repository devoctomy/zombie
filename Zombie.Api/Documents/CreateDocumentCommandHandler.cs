using MediatR;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;

namespace Zombie.Api.Documents
{
    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, CreateDocumentResponse>
    {
        private readonly IRepository<Document> _documentRepository;

        public CreateDocumentCommandHandler(IRepository<Document> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public Task<CreateDocumentResponse> Handle(
            CreateDocumentCommand command,
            CancellationToken cancellationToken)
        {
            var inserted = _documentRepository.Insert(command.Request.Document);
            return Task.FromResult(new CreateDocumentResponse
            {
                IsSuccess = inserted.Status == Repositories.Enums.Status.Success,
                StatusCode = inserted.Status == Repositories.Enums.Status.Success ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                Value = inserted?.Value == null ? null : new Dto.Responses.CreateDocumentResponse
                {
                    Document = inserted.Value
                }
            });
        }
    }
}
