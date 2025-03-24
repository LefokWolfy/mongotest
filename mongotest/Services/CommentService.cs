using MongoDB.Driver;
using mongoAPI.Models;

namespace mongotest.Services
{
    public class CommentService
    {
        private readonly IMongoCollection<Comment> _commentsCollection;

        public CommentService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("forum"); // Change to your database name
            _commentsCollection = database.GetCollection<Comment>("Comments"); // Collection name
        }

        public async Task AddCommentAsync(Comment comment) =>
            await _commentsCollection.InsertOneAsync(comment);

        public async Task<List<Comment>> GetCommentsAsync()
        {
            var comments = await _commentsCollection.Find(comment => true).ToListAsync();

            foreach (var comment in comments)
            {
                comment.Replies = await GetRepliesAsync(comment.CommentId);
            }

            return comments;
        }

        private async Task<List<Comment>> GetRepliesAsync(string parentId)
        {
            var replies = await _commentsCollection.Find(reply => reply.ParentId == parentId).ToListAsync();

            foreach (var reply in replies)
            {
                reply.Replies = await GetRepliesAsync(reply.CommentId);
            }


            return replies;
        }
    }
}
