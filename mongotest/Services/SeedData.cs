using mongoAPI.Models;
using mongoAPI.Services; // Adjust namespace if needed.
using System.Threading.Tasks;

namespace mongotest
{
    public static class SeedData
    {
        public static async Task InitializeAsync(UserService userService, PostService postService, ElasticSearchService elasticSearchService)
        {
            // Create sample users
            var users = new List<User>
            {
                new User { Name = "Alice", Password = "password1" },
                new User { Name = "Bob", Password = "password2" },
                new User { Name = "Carol", Password = "password3" }
            };

            foreach (var user in users)
            {
                await userService.AddUserAsync(user);
            }

            // Create and insert posts, indexing each one in Elasticsearch.
            var posts = new List<Post>();
            for (int i = 1; i <= 10; i++)
            {
                var assignedUser = users[(i - 1) % users.Count];
                posts.Add(new Post
                {
                    Title = $"Test Post {i}",
                    Text = $"This is the content for test post {i}.",
                    UserId = assignedUser.UserId
                });
            }

            foreach (var post in posts)
            {
                await postService.AddPostAsync(post);
                await elasticSearchService.IndexPostAsync(post);
            }
        }
    }
}