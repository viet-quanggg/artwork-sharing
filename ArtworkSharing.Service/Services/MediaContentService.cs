using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Service.Services
{
    public class MediaContentService : IMediaContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MediaContentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<MediaContent>> GetAll()
        {
            return _unitOfWork.MediaContentRepository.GetAll().ToList();
        }

        public async Task<MediaContent> GetOne(Guid MediaContentId)
        {
            return await _unitOfWork.MediaContentRepository
                   .FirstOrDefaultAsync(mc => mc.Id == MediaContentId);
        }

        public async Task Update(MediaContent MediaContentInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var MediaContentRepos = _unitOfWork.MediaContentRepository;
                var MediaContent = await MediaContentRepos.GetAsync(mc => mc.Id == MediaContentInput.Id);
                if (MediaContent == null)
                    throw new KeyNotFoundException();

                MediaContent.Media = MediaContent.Media;

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(MediaContent MediaContentInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var MediaContentRepos = _unitOfWork.MediaContentRepository;
                MediaContentRepos.Add(MediaContentInput);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(Guid MediaContentId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var mediaContentRepository = _unitOfWork.MediaContentRepository;

                var mediaContent = await mediaContentRepository.GetAsync(mc => mc.Id == MediaContentId);

                if (mediaContent == null)
                    throw new KeyNotFoundException();

                mediaContentRepository.Remove(mediaContent);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
