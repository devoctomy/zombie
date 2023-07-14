using MediatR;

namespace Zombie.Api.Documents
{
    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, CreateDocumentResponse>
    {
        public Task<CreateDocumentResponse> Handle(
            CreateDocumentCommand request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
