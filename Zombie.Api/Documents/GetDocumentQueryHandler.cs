using MediatR;
using System.Net;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;
using Zombie.Api.Repositories.Enums;

namespace Zombie.Api.Documents
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, GetDocumentResponse>
    {
        private readonly IRepository<Document> _documentRepository;

        public GetDocumentQueryHandler(IRepository<Document> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public Task<GetDocumentResponse> Handle(
            GetDocumentQuery request,
            CancellationToken cancellationToken)
        {
            var document = _documentRepository.Get(request.Id);
            return Task.FromResult(new GetDocumentResponse
            {
                IsSuccess = document.Status == Status.Success,
                StatusCode = GetHttpStatusCode(document.Status),
                Value = document.Value
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
