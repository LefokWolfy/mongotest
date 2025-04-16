using mongoAPI.Models;
using mongoAPI.Services; // Adjust this namespace if needed.

namespace mongotest
{
    public static class SeedData
    {
        public static async Task InitializeAsync(UserService userService, PostService postService)
        {
            // Create sample users matching your User model.
            var users = new List<User>
            {
                new User { Name = "Alice", Password = "password1" },
                new User { Name = "Bob", Password = "password2" },
                new User { Name = "Carol", Password = "password3" }
            };

            // Insert users into the MongoDB database.
            foreach (var user in users)
            {
                await userService.AddUserAsync(user);
            }

            // Create at least 10 posts, cycling through the available users.
            var posts = new List<Post>();
            for (int i = 1; i <= 10; i++)
            {
                // Cycle through the users for assignment.
                var assignedUser = users[(i - 1) % users.Count];
                posts.Add(new Post
                {
                    Title = $"Test Post {i}",
                    Text = $"This is the content for test post {i}.",
                    UserId = assignedUser.UserId // Assign the ID from the inserted user.
                });
            }

            // Insert posts into the MongoDB database.
            foreach (var post in posts)
            {
                await postService.AddPostAsync(post);
            }
        }
    }
}