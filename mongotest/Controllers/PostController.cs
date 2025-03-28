﻿using Microsoft.AspNetCore.Http;
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
        public PostController(PostService postService) => _postService = postService;

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
            return CreatedAtAction(nameof(CreatePost), new { id = post.PostId }, post);
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts() => Ok(await _postService.GetPostsAsync());
    }
}
