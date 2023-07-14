namespace Zombie.Api.Dto.Requests
{
    public class UpdateDocumentRequest
    {
        public string Header { get; set; } // YAML
        public string Body { get; set; } // Markdown
    }
}
