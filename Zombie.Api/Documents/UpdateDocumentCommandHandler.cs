using MediatR;
using System.Net;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;
using Zombie.Api.Repositories.Enums;

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
                IsSuccess = updated.Status == Status.Success,
                StatusCode = GetHttpStatusCode(updated.Status),
                Value = updated?.Value == null ? null : new Dto.Responses.UpdateDocumentResponse
                {
                    Document = updated.Value
                }
            });
        }

        private static HttpStatusCode GetHttpStatusCode(Status status)
        {
            return status switch
            {
                Status.Success => HttpStatusCode.OK,
                Status.NotFound => HttpStatusCode.NotFound,
                Status.Fail => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
