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
            var inserted = _documentRepository.InsertNew(command.Request.Document);
            return Task.FromResult(new CreateDocumentResponse
            {
                IsSuccess = inserted != null,
                StatusCode = inserted != null ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                Value = inserted == null ? null : new Dto.Responses.CreateDocumentResponse
                {
                    Document = inserted
                }
            });
        }
    }
}
