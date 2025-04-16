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
            await _postService.AddPostAsync(post);
            await _elasticSearchService.IndexPostAsync(post); // Index the post
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
