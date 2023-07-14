using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;
using Zombie.Api.Documents;
using Zombie.Api.Dto.Requests;

namespace Zombie.Api.UnitTests.Documents
{
    public class DocumentControllerTests
    {
        [Fact]
        public async Task GivenCreateDocumentRequest_WhenCreateDocument_ThenCommandSent_AndResponseReturned()
        {
            // Act
            var mockMediator = new Mock<IMediator>();
            var sut = new DocumentController(mockMediator.Object);

            var request = new CreateDocumentRequest
            {
                Key = "Foo/Bar",
                Properties = new Dictionary<string, object>
                {
                    { "Foo", "Bar" },
                    { "Bar", "Foo" }
                }
            };

            var handlerResponse = new CreateDocumentResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Value = new Dto.Responses.CreateDocumentResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Key = request.Key,
                    CreatedAt = DateTime.UtcNow,
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<CreateDocumentCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(handlerResponse);

            // Arrange
            var response = await sut.CreateDocument(request);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var okObjectResult = (OkObjectResult)response;
            var resultValue = (Dto.Responses.CreateDocumentResponse?)okObjectResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(handlerResponse.Value, resultValue);
        }

        [Fact]
        public async Task GivenCreateDocumentRequest_WhenCreateDocument_AndCommandFails_ThenCommandSent_AndResponseReturned()
        {
            // Act
            var mockMediator = new Mock<IMediator>();
            var sut = new DocumentController(mockMediator.Object);

            var request = new CreateDocumentRequest
            {
                Key = "Foo/Bar",
                Properties = new Dictionary<string, object>
                {
                    { "Foo", "Bar" },
                    { "Bar", "Foo" }
                }
            };

            var handlerResponse = new CreateDocumentResponse
            {
                IsSuccess = false,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<CreateDocumentCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(handlerResponse);

            // Arrange
            var response = await sut.CreateDocument(request);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            var statusCodeResult = (StatusCodeResult)response;
            Assert.NotNull(statusCodeResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GivenId_WhenGetDocument_ThenCommandSent_AndResponseReturned()
        {
            // Act
            var mockMediator = new Mock<IMediator>();
            var sut = new DocumentController(mockMediator.Object);

            var id = Guid.NewGuid().ToString();
            var handlerResponse = new GetDocumentResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Value = new Dto.Models.Document
                {
                    Id = id
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetDocumentQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(handlerResponse);

            // Arrange
            var response = await sut.GetDocument(id);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var okObjectResult = (OkObjectResult)response;
            var resultValue = (Dto.Models.Document?)okObjectResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(handlerResponse.Value, resultValue);
        }

        [Fact]
        public async Task GivenId_WhenGetDocument_AndDocumentNotFound_ThenCommandSent_AndResponseReturned()
        {
            // Act
            var mockMediator = new Mock<IMediator>();
            var sut = new DocumentController(mockMediator.Object);

            var id = Guid.NewGuid().ToString();
            var handlerResponse = new GetDocumentResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetDocumentQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(handlerResponse);

            // Arrange
            var response = await sut.GetDocument(id);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            var statusCodeResult = (StatusCodeResult)response;
            Assert.NotNull(statusCodeResult);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }
    }
}
