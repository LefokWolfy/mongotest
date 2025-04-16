using MongoDB.Driver;
using mongoAPI.Models;

namespace mongoAPI.Services
{
    public class PostService
    {
        // Readonly so it cant be reassigned
        private readonly IMongoCollection<Post> _postsCollection;
        private readonly IMongoCollection<Comment> _commentsCollection;
        private readonly IMongoCollection<User> _usersCollection;


        public PostService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("forum"); // Change to your database name
            _postsCollection = database.GetCollection<Post>("Posts"); // Collection name
            _commentsCollection = database.GetCollection<Comment>("Comments"); // Collection name
            _usersCollection = database.GetCollection<User>("Users"); // Collection name

        }

        public async Task AddPostAsync(Post post)
        {
            await _postsCollection.InsertOneAsync(post);
            Console.WriteLine($"[DEBUG] Inserted post with ID: {post.PostId}");

            var updateResult = await _usersCollection.UpdateOneAsync(
                user => user.UserId == post.UserId,
                Builders<User>.Update.Push(user => user.PostIds, post.PostId));

            if(updateResult.ModifiedCount == 0)
            {
                Console.WriteLine($"[DEBUG] User update did not modify any document for UserId: {post.UserId}");
            }
        }


        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            //var posts = await _postsCollection.Find(post => true).ToListAsync();

            //foreach (var post in posts)
            //{
            //    var comments = await _commentsCollection.Find(comment => comment.PostId == post.PostId).ToListAsync();
            //    post.Comments = comments;
            //}

            //return posts;

            return await _postsCollection.Find(post => true).ToListAsync();
        }
    }
}
