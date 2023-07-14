using System.Text.Json;
using Xunit;
using YamlDotNet.Core;
using Zombie.Api.Services;

namespace Zombie.Api.UnitTests.Services
{
    public class MarkdownWithYamlFrontmatterDocumentParserTests
    {
        [Theory]
        [InlineData("Data/Input/Valid/Document1.md")]
        [InlineData("Data/Input/Valid/Document2.md")]
        public async Task GivenValidMarkdown_WhenValidate_ThenDocumentParsedCorrectly(string documentPath)
        {
            // Arrange
            var documentContent = await File.ReadAllTextAsync(documentPath);
            var sut = new MarkdownWithYamlFrontmatterDocumentParser();

            // Act & Assert
            sut.Validate(documentContent);
        }

        [Theory]
        [InlineData("Data/Input/Invalid/BadFrontmatter.md", typeof(YamlException))]
        public async Task GivenInvalidMarkdown_WhenValidate_ThenExceptionThrown(
            string documentPath,
            Type exceptionType)
        {
            // Arrange
            var documentContent = await File.ReadAllTextAsync(documentPath);
            var sut = new MarkdownWithYamlFrontmatterDocumentParser();

            // Act & Assert
            Assert.Throws(exceptionType, () =>
            {
                sut.Validate(documentContent);
            });
        }

        [Theory]
        [InlineData(
            "Data/Input/Valid/Document1.md",
            "Data/Output/Properties_Document1.json",
            "Data/Output/Body_Document1.md")]
        public async Task GivenValidMarkdown_WhenParse_ThenDocumentParsedCorrectly(
            string documentPath,
            string expectedPropertiesPath,
            string expectedBodyPath)
        {
            // Arrange
            var documentContent = await File.ReadAllTextAsync(documentPath);
            var expectedPropeties = await File.ReadAllTextAsync(expectedPropertiesPath);
            var expectedBody = await File.ReadAllTextAsync(expectedBodyPath);
            var sut = new MarkdownWithYamlFrontmatterDocumentParser();

            // Act
            var document = sut.Parse(documentContent);

            // Assert
            var actualProperties = JsonSerializer.Serialize(
                document.Properties,
                new JsonSerializerOptions { WriteIndented = true });
            Assert.Equal(expectedPropeties, actualProperties);
            Assert.Equal(expectedBody, document.Body);
        }
    }
}
