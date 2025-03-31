using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoAPI.Models
{
    public class Post
    {
        [BsonId] // Primary key
        // Need to use this so it serializes the ObjectId to a string properly.
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        // Here we need it again because otherwise it will serialize the ObjectId to a object which messes with the timestamp and such.
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public List<string>? CommentIds { get; set; } = new List<string>();
    }

    public class PostDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

    }
}
