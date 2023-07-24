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
            var mockIoService = new Mock<IIoService>();
            var mockDocumentParser = new Mock<IDocumentParser>();

            // Act & Assert
            Assert.ThrowsAny<FileSystemRepositoryMissingBasePathOption>(() =>
            {
                var sut = new FileSystemDocumentRepository(
                    options,
                    mockIoService.Object,
                    mockDocumentParser.Object);
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
            var mockIoService = new Mock<IIoService>();
            var mockDocumentParser = new Mock<IDocumentParser>();

            // Act & Assert
            Assert.ThrowsAny<FileSystemRepositoryBasePathNotWritableException>(() =>
            {
                var sut = new FileSystemDocumentRepository(
                    options,
                    mockIoService.Object,
                    mockDocumentParser.Object);
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
            var mockIoService = new Mock<IIoService>();
            var mockDocumentParser = new Mock<IDocumentParser>();
            Directory.CreateDirectory(options.BasePath);

            // Act & Assert
            var sut = new FileSystemDocumentRepository(
                options,
                mockIoService.Object,
                mockDocumentParser.Object);

            // Cleanup
            Directory.Delete(options.BasePath);
        }

        [Fact]
        public async Task GivenKey_WhenGet_ThenDocumentParsed_AndSuccessReturned()
        {
            // Arrange
            var options = new FileSystemRepositoryOptions
            {
                BasePath = "TestRepo"
            };
            var key = "Foo/Bar/FooBar.md";
            var combinedPath = "TestRepo/Foo/Bar/FooBar.md";
            var mockIoService = new Mock<IIoService>();
            var mockDocumentParser = new Mock<IDocumentParser>();
            var sut = new FileSystemDocumentRepository(
                options,
                mockIoService.Object,
                mockDocumentParser.Object);

            mockIoService.Setup(x => x.CombinePath(
                It.IsAny<string[]>()))
                .Returns(combinedPath);

            mockIoService.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>()))
                .ReturnsAsync("Hello World!");

            mockDocumentParser.Setup(x => x.Parse(
                It.IsAny<string>()))
                .Returns(new Dto.Models.Document());

            // Act
            var result = await sut.Get(key);

            // Assert
            mockIoService.Verify(x => x.ReadAllTextAsync(
                It.Is<string>(y => y == combinedPath)), Times.Once);

            mockDocumentParser.Verify(x => x.Parse(
                It.Is<string>(y => y == "Hello World!")), Times.Once);
            Assert.Equal(Api.Repositories.Enums.Status.Success, result.Status);
        }
    }
}
