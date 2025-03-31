using MongoDB.Driver;
using mongoAPI.Models;

namespace mongotest.Services
{
    public class CommentService
    {
        private readonly IMongoCollection<Comment> _commentsCollection;
        private readonly IMongoCollection<Post> _postsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public CommentService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("forum"); // Change to your database name
            _commentsCollection = database.GetCollection<Comment>("Comments"); // Collection name
            _postsCollection = database.GetCollection<Post>("Posts"); // Collection name
            _usersCollection = database.GetCollection<User>("Users"); // Collection name
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentsCollection.InsertOneAsync(comment);

            if (comment.ParentId != null)
            {
                await _commentsCollection.UpdateOneAsync(c => c.CommentId == comment.ParentId, Builders<Comment>.Update.Push(c => c.ReplyIds, comment.CommentId));
            }

            await _postsCollection.UpdateOneAsync(post => post.PostId == comment.PostId, Builders<Post>.Update.Push(post => post.CommentIds, comment.CommentId));
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            //var comments = await _commentsCollection.Find(comment => true).ToListAsync();

            //foreach (var comment in comments)
            //{
            //    comment.Replies = await GetRepliesAsync(comment.CommentId);
            //}

            //return comments;

            return await _commentsCollection.Find(comment => true).ToListAsync();
        }

        //private async Task<List<Comment>> GetRepliesAsync(string parentId)
        //{
        //    var replies = await _commentsCollection.Find(reply => reply.ParentId == parentId).ToListAsync();

        //    foreach (var reply in replies)
        //    {
        //        reply.Replies = await GetRepliesAsync(reply.CommentId);
        //    }


        //    return replies;
        //}
    }
}

