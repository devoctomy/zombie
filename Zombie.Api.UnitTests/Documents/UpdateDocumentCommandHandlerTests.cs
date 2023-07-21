using Moq;
using Xunit;
using Zombie.Api.Documents;
using Zombie.Api.Dto.Models;
using Zombie.Api.Repositories;

namespace Zombie.Api.UnitTests.Documents
{
    public class UpdateDocumentCommandHandlerTests
    {
        [Fact]
        public async Task GivenUpdateDocumentCommand_WhenHandled_ThenDocumentUpdated_AndResponseReturned()
        {
            // Arrange
            var mockDocumentRepository = new Mock<IRepository<Document>>();
            var sut = new UpdateDocumentCommandHandler(mockDocumentRepository.Object);

            var command = new UpdateDocumentCommand(
                new Dto.Requests.UpdateDocumentRequest
                {
                    Document = new Document()
                });

            var updatedAt = new DateTime(2023, 01, 01);
            mockDocumentRepository.Setup(x => x.Update(
                It.IsAny<Document>()))
                .Returns(new RepositoryResponse<Document>(
                    Api.Repositories.Enums.Status.Success,
                    new Document
                    {
                        UpdatedAt = updatedAt,
                    }));

            // Act
            var response = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(updatedAt, response.Value?.Document?.UpdatedAt);
        }

        [Fact]
        public async Task GivenUpdateDocumentCommand_AndDocumentNotExists_WhenHandled_ThenDocumentNotUpdated_AndResponseReturned()
        {
            // Arrange
            var mockDocumentRepository = new Mock<IRepository<Document>>();
            var sut = new UpdateDocumentCommandHandler(mockDocumentRepository.Object);

            var command = new UpdateDocumentCommand(
                new Dto.Requests.UpdateDocumentRequest
                {
                    Document = new Document()
                });

            var updatedAt = new DateTime(2023, 01, 01);
            mockDocumentRepository.Setup(x => x.Update(
                It.IsAny<Document>()))
                .Returns(new RepositoryResponse<Document>(Api.Repositories.Enums.Status.NotFound, null));

            // Act
            var response = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Null(response?.Value?.Document);
        }
    }
}
