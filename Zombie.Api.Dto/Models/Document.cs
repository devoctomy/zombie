namespace Zombie.Api.Dto.Models
{
    public class Document : Entity
    {
        public Dictionary<string, object>? Properties { get; set; }
        public string? Body { get; set; }
    }
}
