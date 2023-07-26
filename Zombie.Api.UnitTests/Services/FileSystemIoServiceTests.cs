using Xunit;
using Zombie.Api.Services;

namespace Zombie.Api.UnitTests.Services
{
    public class FileSystemIoServiceTests
    {
        [Theory]
        [InlineData("Foo", "Bar/Foo/Bar/pop.pop", "Foo/Bar/Foo/Bar/pop.pop")]
        public void GivenPathParts_WhenCombinePath_ThenCombinedPathReturned(
            string path1,
            string path2,
            string expectedCombinedPath)
        {
            // Arrange
            var sut = new FileSystemIoService();

            // Act
            var result = sut.CombinePath(path1, path2);

            // Assert
            Assert.Equal(expectedCombinedPath, result);
        }

        [Fact]
        public async Task GivenExistingFile_WhenExists_ThenTrueReturned()
        {
            // Act
            var path = $"{Guid.NewGuid()}.tmp";
            var sut = new FileSystemIoService();
            await File.WriteAllTextAsync(path, "Hello World!");

            // Arrange
            var exists = sut.Exists(path);

            // Assert
            Assert.True(exists);

            // Cleanup
            File.Delete(path);
        }

        [Fact]
        public void GivenNonExistantFile_WhenExists_ThenFalseReturned()
        {
            // Act
            var path = $"{Guid.NewGuid()}.tmp";
            var sut = new FileSystemIoService();

            // Arrange
            var exists = sut.Exists(path);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task GivenExistingFile_WhenReadAllTextAsync_ThenFileDataReturned()
        {
            // Act
            var content = "Hello World!";
            var path = $"{Guid.NewGuid()}.tmp";
            var sut = new FileSystemIoService();
            await File.WriteAllTextAsync(path, content);

            // Arrange
            var data = await sut.ReadAllTextAsync(path);

            // Assert
            Assert.Equal(content, data);

            // Cleanup
            File.Delete(path);
        }

        [Fact]
        public async Task GivenNewFile_AndContent_WhenWriteAllTextAsync_ThenContentWrittentToFile()
        {
            // Act
            var content = "Hello World!";
            var path = $"{Guid.NewGuid()}.tmp";
            var sut = new FileSystemIoService();

            // Arrange
            await sut.WriteAllTextAsync(path, content);

            // Assert
            var data = await File.ReadAllTextAsync(path);
            Assert.Equal(content, data);

            // Cleanup
            File.Delete(path);
        }

        [Fact]
        public async Task GivenExistingFile_WhenDelete_ThenFileDeleted()
        {
            // Act
            var content = "Hello World!";
            var path = $"{Guid.NewGuid()}.tmp";
            var sut = new FileSystemIoService();
            await File.WriteAllTextAsync(path, content);

            // Arrange
            sut.Delete(path);

            // Assert
            Assert.False(File.Exists(path));
        }
    }
}
