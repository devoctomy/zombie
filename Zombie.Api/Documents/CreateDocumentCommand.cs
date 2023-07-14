using MediatR;
using Zombie.Api.Dto.Requests;

namespace Zombie.Api.Documents
{
    public class CreateDocumentCommand : IRequest<CreateDocumentResponse>
    {
        public CreateDocumentRequest Request { get; set; }

        public CreateDocumentCommand(CreateDocumentRequest request)
        {
            Request = request;
        }
    }
}
