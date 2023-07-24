using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
