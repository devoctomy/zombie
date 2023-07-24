using Moq;
using Xunit;
using Zombie.Api.Documents;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;

namespace Zombie.Api.UnitTests.Documents
{
    public class CreateDocumentCommandHandlerTests
    {
        [Fact]
        public async Task GivenCreateDocumentCommand_WhenHandle_ThenDocumentInserted_AndResultReturned()
        {
            // Arrange
            var mockDocumentRepository = new Mock<IRepository<Document>>();
            var sut = new CreateDocumentCommandHandler(mockDocumentRepository.Object);

            var command = new CreateDocumentCommand(
                new Dto.Requests.CreateDocumentRequest
                {
                    Document = new Document
                    {
                        Key = "Foo/Bar",
                        Properties = new Dictionary<string, object>
                        {
                            { "Foo", "Bar" },
                            { "Bar", "Foo" }
                        },
                        Body = "Hello World!"
                    }
                });
            var document = new Document
            {
            };

            mockDocumentRepository.Setup(x => x.Insert(
                It.IsAny<Document>()))
                .ReturnsAsync(new RepositoryResponse<Document>(Api.Repositories.Enums.Status.Success, document));

            // Act
            var response = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(document, response?.Value?.Document);
        }

        [Fact]
        public async Task GivenCreateDocumentCommand_WhenHandle_AndInsertFailed_ThenDocumentNotInserted_AndResultReturned()
        {
            // Arrange
            var mockDocumentRepository = new Mock<IRepository<Document>>();
            var sut = new CreateDocumentCommandHandler(mockDocumentRepository.Object);

            var command = new CreateDocumentCommand(
                new Dto.Requests.CreateDocumentRequest
                {
                    Document = new Document
                    {
                        Key = "Foo/Bar",
                        Properties = new Dictionary<string, object>
                        {
                            { "Foo", "Bar" },
                            { "Bar", "Foo" }
                        },
                        Body = "Hello World!"
                    }
                });

            mockDocumentRepository.Setup(x => x.Insert(
                It.IsAny<Document>()))
                .ReturnsAsync(new RepositoryResponse<Document>(Api.Repositories.Enums.Status.NotFound, null));

            // Act
            var response = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Null(response.Value);
        }
    }
}
