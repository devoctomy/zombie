using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using System.Dynamic;
using YamlDotNet.Serialization;

namespace Zombie.Api.Services
{
    public class MarkdownWithYamlFrontmatterDocumentParser : IDocumentParser
    {
        private static readonly IDeserializer YamlDeserializer =
            new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();

        private static readonly MarkdownPipeline Pipeline
            = new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .Build();

        public Dto.Requests.Document Parse(string markdown)
        {
            var markdownDocument = Markdown.Parse(markdown, Pipeline);
            var yamlBlock = markdownDocument
                .Descendants<YamlFrontMatterBlock>()
                .SingleOrDefault();
            var document = new Dto.Requests.Document();

            if (yamlBlock != null)
            {
                var rawYaml = markdown.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length);
                rawYaml = rawYaml.Trim('-');
                var sr = new StringReader(rawYaml);
                dynamic yamlExpando = YamlDeserializer.Deserialize<ExpandoObject>(sr);
                document.Properties = ((IDictionary<string,object>)yamlExpando).ToDictionary(x => x.Key, x => x.Value);
            }

            document.Body = markdown.Substring(yamlBlock != null ? yamlBlock.Span.End + 1 : 0).Trim('\r', '\n', '\t', ' ');
            return document;
        }

        public void Validate(string markdown)
        {
            var markdownDocument = Markdown.Parse(markdown, Pipeline);
            var yamlBlock = markdownDocument
                .Descendants<YamlFrontMatterBlock>()
                .SingleOrDefault();

            if(yamlBlock != null)
            {
                var deserializer = new Deserializer();
                var rawYaml = markdown.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length);
                rawYaml = rawYaml.Trim('-');
                var sr = new StringReader(rawYaml);
                dynamic yamlExpando = deserializer.Deserialize<ExpandoObject>(sr);
            }
        }
    }
}
