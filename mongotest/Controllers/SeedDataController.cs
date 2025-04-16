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
    }
}