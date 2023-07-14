using System;
using System.Collections.Generic;
using System.Linq;
namespace Zombie.Api.Dto.Responses
{
    public class UpdateDocumentResponse
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
