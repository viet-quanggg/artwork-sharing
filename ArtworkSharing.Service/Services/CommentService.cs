﻿using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Comments;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CommentViewModel>> Add(Guid artworkId, Guid userId, string content)
    {
        Comment cmt = new Comment
        {
            ArtworkId = artworkId,
            CommentedUserId = userId,
            Content = content,
            Id = Guid.NewGuid()
        };
        cmt.CommentedDate = DateTime.Now;
        var commentRepository = _unitOfWork.CommentRepository;
        await commentRepository.AddAsync(cmt);
        await _unitOfWork.SaveChangesAsync();
        return await GetCommentByArtworkId(cmt.ArtworkId);
    }

    public async Task<bool> Delete(Guid commentId)
    {
        var commentRepository = _unitOfWork.CommentRepository;
        var comment = await commentRepository.GetAsync(c => c.Id == commentId);
        if (comment == null)
            throw new KeyNotFoundException();

        await commentRepository.DeleteAsync(comment);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<List<CommentViewModel>> GetAll()
    {
        return AutoMapperConfiguration.Mapper.Map<List<CommentViewModel>>(await _unitOfWork.CommentRepository
            .GetAllAsync());
    }


    public async Task<List<CommentViewModel>> GetCommentByArtworkId(Guid id)
    {
        return AutoMapperConfiguration.Mapper.Map<List<CommentViewModel>>(await _unitOfWork.CommentRepository.Include(x => x.CommentedUser)
            .Where(x => x.ArtworkId == id).ToListAsync());
    }

    public async Task<Comment> GetOne(Guid commentId)
    {
        return await _unitOfWork.CommentRepository.FindAsync(commentId);
    }

    public async Task<CommentViewModel> GetComment(Guid commentId)
    {
        return AutoMapperConfiguration.Mapper.Map<CommentViewModel>(
            await _unitOfWork.CommentRepository.FirstOrDefaultAsync(x => x.Id == commentId));
    }

    public async Task<CommentViewModel> Update(UpdateCommentModel comment)
    {
        var commentRepository = _unitOfWork.CommentRepository;
        var existingComment = await commentRepository.FirstOrDefaultAsync(x => x.Id == comment.Id);
        if (existingComment == null)
            throw new KeyNotFoundException();

        existingComment.Content = comment.Content;

        return await _unitOfWork.SaveChangesAsync() > 0 ? await GetComment(comment.Id) : null!;
    }
}