namespace Zombie.Api.Dto.Models
{
    public class Document : Entity
    {
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public string Body { get; set; }
    }
}
