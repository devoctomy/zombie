namespace Zombie.Api.Dto.Requests
{
    public class CreateDocumentRequest
    {
        public string Key { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public string Body { get; set; }
    }
}
