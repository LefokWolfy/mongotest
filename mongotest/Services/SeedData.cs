using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mongoAPI.Models;
using mongoAPI.Services; // adjust namespace as needed

namespace mongotest
{
    public static class SeedData
    {
        public static async Task InitializeAsync(UserService userService, PostService postService, ElasticSearchService elasticSearchService)
        {
            try
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
                    Console.WriteLine($"User '{user.Name}' inserted with ID: {user.UserId}");
                }

                // Create and insert posts, and then index each one in Elasticsearch.
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
                    // Insert the post into MongoDB.
                    await postService.AddPostAsync(post);
                    Console.WriteLine($"Post '{post.Title}' inserted with ID: {post.PostId}");

                    // Index the post into Elasticsearch and log the response.
                    var indexResponse = await elasticSearchService.IndexPostAsync(post);
                    if (indexResponse.IsValid)
                    {
                        Console.WriteLine($"Post '{post.Title}' indexed in Elasticsearch successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Elasticsearch indexing failed for post '{post.Title}': {indexResponse.DebugInformation}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during seeding: {ex.Message}");
            }
        }
    }
}