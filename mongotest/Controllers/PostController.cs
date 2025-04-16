using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mongoAPI.Models;
using mongoAPI.Services;

namespace mongotest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly ElasticSearchService _elasticSearchService;

        public PostController(PostService postService, ElasticSearchService elasticSearchService)
        {
            _postService = postService;
            _elasticSearchService = elasticSearchService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDTO postDTO)
        {
            var post = new Post
            {
                Title = postDTO.Title,
                Text = postDTO.Text,
                UserId = postDTO.UserId
            };

            // Optionally pre-assign an ID if needed:
            if (string.IsNullOrEmpty(post.PostId))
            {
                post.PostId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            }

            await _postService.AddPostAsync(post);
            Console.WriteLine($"[DEBUG] Post inserted with ID: {post.PostId}");
            
            await _elasticSearchService.IndexPostAsync(post);
            Console.WriteLine($"[DEBUG] Post indexed in ES: {post.PostId}");

            return CreatedAtAction(nameof(CreatePost), new { id = post.PostId }, post);
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts() => Ok(await _postService.GetPostsAsync());

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var searchResponse = await _elasticSearchService.SearchPostsAsync(q);
            return Ok(searchResponse.Documents);
        }
    }
}
