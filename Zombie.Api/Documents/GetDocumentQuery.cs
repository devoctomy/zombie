using MediatR;

namespace Zombie.Api.Documents
{
    public class GetDocumentQuery : IRequest<GetDocumentResponse>
    {
        public string Id { get; }

        public GetDocumentQuery(string id)
        {
            Id = id;
        }
    }
}
