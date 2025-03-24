using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class UserDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
