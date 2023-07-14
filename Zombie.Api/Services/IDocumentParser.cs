using Zombie.Api.Dto.Requests;

namespace Zombie.Api.Services
{
    public interface IDocumentParser
    {
        public void Validate(string markdown);
        public Document Parse(string markdown);
    }
}
