using Moq;
using Xunit;
using Zombie.Api.Exceptions;
using Zombie.Api.Repositories;
using Zombie.Api.Services;

namespace Zombie.Api.UnitTests.Repositories
{
    public class FileSystemRepositoryTests
    {
        [Fact]
        public void GivenOptions_AndBasePathNotSet_WhenInstantiateInstance_ThenFileSystemRepositoryMissingBasePathOptionThrown()
        {
            // Arrange
            var options = new FileSystemRepositoryOptions();
            var mockDocumentParser = new Mock<IDocumentParser>();

            // Act & Assert
            Assert.ThrowsAny<FileSystemRepositoryMissingBasePathOption>(() =>
            {
                var sut = new FileSystemDocumentRepository(options, mockDocumentParser.Object);
            });
        }

        [Fact]
        public void GivenOptions_AndBasePathNotExists_WhenInstantiateInstance_ThenFileSystemRepositoryBasePathNotWritableExceptionThrown()
        {
            // Arrange
            var options = new FileSystemRepositoryOptions
            {
                BasePath = "FolderNotExists"
            };
            var mockDocumentParser = new Mock<IDocumentParser>();

            // Act & Assert
            Assert.ThrowsAny<FileSystemRepositoryBasePathNotWritableException>(() =>
            {
                var sut = new FileSystemDocumentRepository(options, mockDocumentParser.Object);
            });
        }

        [Fact]
        public void GivenOptions_AndBasePathExists_WhenInstantiateInstance_ThenInstanceInstantiated()
        {
            // Arrange
            var options = new FileSystemRepositoryOptions
            {
                BasePath = Guid.NewGuid().ToString()
            };
            var mockDocumentParser = new Mock<IDocumentParser>();
            Directory.CreateDirectory(options.BasePath);

            // Act & Assert
            var sut = new FileSystemDocumentRepository(options, mockDocumentParser.Object);

            // Cleanup
            Directory.Delete(options.BasePath);
        }

        [Fact]
        public async Task GivenKey_WhenGet_ThenDocumentReturned()
        {
            // Arrange
            var options = new FileSystemRepositoryOptions
            {
                BasePath = "TestRepo"
            };
            var mockDocumentParser = new Mock<IDocumentParser>();
            var sut = new FileSystemDocumentRepository(options, mockDocumentParser.Object);

            // Act
            var result = await sut.Get("Foo/Bar/FooBar.md");

            // Assert
            Assert.Equal(Api.Repositories.Enums.Status.Success, result.Status);
        }
    }
}
