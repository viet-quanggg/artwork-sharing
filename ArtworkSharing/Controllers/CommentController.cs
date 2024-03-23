using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Comments;
using ArtworkSharing.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtworkSharing.Controllers;

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
    ///     Get comment by artworkId
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
    ///     Create comment
    /// </summary>
    /// <param name="createCommentModel"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment(CreateCommentModel createCommentModel)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();

        if (createCommentModel == null) return BadRequest();

        var rs = await _commentService.Add(createCommentModel.ArtworkId, uid, createCommentModel.Content);
        return rs != null!
            ? StatusCode(StatusCodes.Status201Created, rs)
            : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Create failed" });
    }

    /// <summary>
    ///     Delete comment
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
    {
        var idRaw = HttpContext.Items["UserId"];
        if (idRaw == null) return Unauthorized();

        Guid uid = Guid.Parse(idRaw + "");

        if (uid == Guid.Empty) return Unauthorized();

        var comment = await _commentService.GetOne(id);

        if (comment == null) return BadRequest();

        if (comment.CommentedUserId != uid) return BadRequest();

        if (id == Guid.Empty) return BadRequest(new { Message = "Not found comment" });
        return await _commentService.Delete(id)
            ? StatusCode(StatusCodes.Status204NoContent)
            : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete failed" });
    }

    /// <summary>
    ///     Update comment
    /// </summary>
    /// <param name="updateCommentModel"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateComment(UpdateCommentModel updateCommentModel)
    {
        var uidClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid uid = new Guid(uidClaim!.Value);

        if (uid == Guid.Empty) return Unauthorized();

        var comment = await _commentService.GetOne(updateCommentModel.Id);

        if (comment == null) return BadRequest();

        if (comment.CommentedUserId != uid) return BadRequest();

        if (updateCommentModel == null || updateCommentModel.Id == Guid.Empty) return BadRequest();
        var rs = await _commentService.Update(updateCommentModel);
        return rs != null!
            ? Ok(rs)
            : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update failed" });
    }
}