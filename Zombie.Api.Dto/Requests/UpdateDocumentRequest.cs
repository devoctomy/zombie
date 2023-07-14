namespace Zombie.Api.Dto.Requests
{
    public class UpdateDocumentRequest
    {
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public string Body { get; set; }
    }
}
