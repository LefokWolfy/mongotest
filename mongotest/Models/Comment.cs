using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoAPI.Models
{
    public class Comment
    {
        [BsonId]
        public ObjectId CommentId { get; set; }
        public string CommentText { get; set; }
        public List<Comment>? Replies { get; set; }
    }

    public class CommentDTO
    {
        public string CommentText { get; set; }
    }
}
