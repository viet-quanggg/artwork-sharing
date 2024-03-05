using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentByArtworkId([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest(new { Message = "Not found artwork" });
            return Ok(await _commentService.GetCommentByArtworkId(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentModel ccm)
        {
            if (ccm == null) return BadRequest();

            var rs = await _commentService.Add(ccm);
            return rs != null! ? StatusCode(StatusCodes.Status201Created, rs)
                : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Create failed" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest(new { Message = "Not found comment" });
            return (await _commentService.Delete(id)) ? StatusCode(StatusCodes.Status204NoContent)
                : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete failed" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentModel ucm)
        {
            if (ucm == null || ucm.Id == Guid.Empty) return BadRequest();
            var rs = await _commentService.Update(ucm);
            return rs != null! ? Ok(rs)
                  : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update failed" });
        }
    }
}
