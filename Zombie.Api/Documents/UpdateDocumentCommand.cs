using MediatR;
using Zombie.Api.Dto.Requests;

namespace Zombie.Api.Documents
{
    public class UpdateDocumentCommand : IRequest<UpdateDocumentResponse>
    {
        public UpdateDocumentRequest Request { get; }

        public UpdateDocumentCommand(UpdateDocumentRequest request)
        {
            Request = request;
        }
    }
}
