using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zombie.Api.Controller;
using Zombie.Api.Dto.Requests;

namespace Zombie.Api.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : BaseController
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> CreateDocument([FromBody]CreateDocumentRequest request)
        {
            var command = new CreateDocumentCommand(request);

            var response = await _mediator.Send(command);
            return ProcessResponse(response);
        }

        // Create
        // Read
        // Update
        // Delete

    }
}
