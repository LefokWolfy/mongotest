using Microsoft.AspNetCore.Mvc;
using mongotest;
using mongoAPI.Services;

namespace mongotest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly ElasticSearchService _elasticSearchService;

        public SeedDataController(UserService userService, PostService postService, ElasticSearchService elasticSearchService)
        {
            _userService = userService;
            _postService = postService;
            _elasticSearchService = elasticSearchService;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            await SeedData.InitializeAsync(_userService, _postService, _elasticSearchService);
            return Ok("Seed complete");
        }

        [HttpPost("reindex")]
        public async Task<IActionResult> ReindexAll()
        {
            var allPosts = await _postService.GetPostsAsync();
            foreach (var post in allPosts)
            {
                var indexResponse = await _elasticSearchService.IndexPostAsync(post);
                if (!indexResponse.IsValid)
                {
                    return StatusCode(500, $"Failed to reindex post {post.PostId}: {indexResponse.DebugInformation}");
                }
            }
            return Ok("Reindex complete");
        }

    }
}