using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mongoAPI.Services;
using mongoAPI.Models;
using mongotest.Services;

namespace mongotest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentsController(CommentService commentService) => _commentService = commentService;

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO commentDTO)
        {
            var comment = new Comment
            {
                CommentText = commentDTO.CommentText,
                UserId = commentDTO.UserId,
                PostId = commentDTO.PostId,
                ParentId = commentDTO.ParentId
            };
            await _commentService.AddCommentAsync(comment);
            return CreatedAtAction(nameof(CreateComment), new { id = comment.CommentId }, comment);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments() => Ok(await _commentService.GetCommentsAsync());

    }
}
