using Zombie.Api.Dto.Models;

namespace Zombie.Api.Services
{
    public interface IDocumentParser
    {
        public void Validate(string markdown);
        public Document Parse(string markdown);
        public string Serialise(Document document);
    }
}
