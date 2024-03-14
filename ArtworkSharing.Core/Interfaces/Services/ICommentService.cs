using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Comments;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetAll();
    Task<Comment> GetOne(Guid commentId);
    Task<CommentViewModel> GetComment(Guid commentId);
    Task<List<CommentViewModel>> GetCommentByArtworkId(Guid id);
    Task<CommentViewModel> Update(UpdateCommentModel comment);
    Task<List<CommentViewModel>> Add(CreateCommentModel comment);
    Task<bool> Delete(Guid commentId);
}