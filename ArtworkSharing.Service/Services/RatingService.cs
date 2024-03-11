using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtworkSharing.Core.ViewModels.MediaContent;
using ArtworkSharing.Service.AutoMappings;
using ArtworkSharing.Core.ViewModels.Rating;
using ArtworkSharing.Core.ViewModels.Package;
using ArtworkSharing.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RatingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<RatingViewModel>> GetAll()
        {
            var queryableData = _unitOfWork.RatingRepository.GetAll().AsQueryable();

            return AutoMapperConfiguration.Mapper.Map<IList<RatingViewModel>>(await queryableData.ToListAsync(CancellationToken.None));
        }

        public async Task<RatingViewModel> GetOne(Guid RatingId)
        {
            return AutoMapperConfiguration.Mapper.Map<RatingViewModel>(await (_unitOfWork.PackageRepository.FirstOrDefaultAsync(x => x.Id == RatingId)));

        }

        public async Task Update(Rating RatingInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var RatingRepos = _unitOfWork.RatingRepository;
                var Rating = await RatingRepos.GetAsync(mc => mc.Id == RatingInput.Id);
                if (Rating == null)
                    throw new KeyNotFoundException();

                Rating.Content = Rating.Content;

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(Rating RatingInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var RatingRepos = _unitOfWork.RatingRepository;
                RatingRepos.Add(RatingInput);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(Guid RatingId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var RatingRepository = _unitOfWork.RatingRepository;

                var Rating = await RatingRepository.GetAsync(mc => mc.Id == RatingId);

                if (Rating == null)
                    throw new KeyNotFoundException();

                RatingRepository.DeleteAsync(Rating);

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
