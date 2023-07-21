using MediatR;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;

namespace Zombie.Api.Documents
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, UpdateDocumentResponse>
    {
        private readonly IRepository<Document> _documentRepository;

        public UpdateDocumentCommandHandler(IRepository<Document> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public Task<UpdateDocumentResponse> Handle(
            UpdateDocumentCommand command,
            CancellationToken cancellationToken)
        {
            var updated = _documentRepository.Update(command.Request.Document);
            return Task.FromResult(new UpdateDocumentResponse
            {
                IsSuccess = updated != null,
                StatusCode = updated != null ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                Value = updated == null ? null : new Dto.Responses.UpdateDocumentResponse
                {
                    Document = updated
                }
            });
        }
    }
}
