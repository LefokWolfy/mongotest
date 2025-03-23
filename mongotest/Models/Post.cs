using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongoAPI.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        List<Comment>? Comments { get; set; }
    }

    public class PostDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
