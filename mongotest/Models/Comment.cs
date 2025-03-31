using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoAPI.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentId { get; set; }
        public string CommentText { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? PostId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentId { get; set; } // Parent comment

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public List<string>? ReplyIds { get; set; } = new List<string>();
    }

    public class CommentDTO
    {
        public string CommentText { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? PostId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentId { get; set; }

    }
}
