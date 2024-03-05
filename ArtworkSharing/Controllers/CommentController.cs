using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Comments;
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

        /// <summary>
        /// Get comment by artworkId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentByArtworkId([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest(new { Message = "Not found artwork" });
            return Ok(await _commentService.GetCommentByArtworkId(id));
        }

        /// <summary>
        /// Create comment
        /// </summary>
        /// <param name="createCommentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentModel createCommentModel)
        {
            if (createCommentModel == null) return BadRequest();

            var rs = await _commentService.Add(createCommentModel);
            return rs != null! ? StatusCode(StatusCodes.Status201Created, rs)
                : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Create failed" });
        }

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest(new { Message = "Not found comment" });
            return (await _commentService.Delete(id)) ? StatusCode(StatusCodes.Status204NoContent)
                : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete failed" });
        }

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="updateCommentModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentModel updateCommentModel)
        {
            if (updateCommentModel == null || updateCommentModel.Id == Guid.Empty) return BadRequest();
            var rs = await _commentService.Update(updateCommentModel);
            return rs != null! ? Ok(rs)
                  : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update failed" });
        }
    }
}
