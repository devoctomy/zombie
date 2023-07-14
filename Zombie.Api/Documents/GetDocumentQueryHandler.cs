using MediatR;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;

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
                IsSuccess = document != null,
                StatusCode = document != null ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                Value = document
            });
        }
    }
}
