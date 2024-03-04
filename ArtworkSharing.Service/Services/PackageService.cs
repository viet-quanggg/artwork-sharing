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
using ArtworkSharing.Core.ViewModels.Package;
using ArtworkSharing.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<PackageViewModel>> GetAll()
        {
            var queryableData = _unitOfWork.PackageRepository.GetAll().AsQueryable();

            return AutoMapperConfiguration.Mapper.Map<IList<PackageViewModel>>(await queryableData.ToListAsync(CancellationToken.None));
        }

        public async Task<PackageViewModel> GetOne(Guid PackageId)
        {
            return AutoMapperConfiguration.Mapper.Map<PackageViewModel>(await (_unitOfWork.PackageRepository.FirstOrDefaultAsync(x => x.Id == PackageId)));

        }

        public async Task Update(Package PackageInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var PackageRepos = _unitOfWork.PackageRepository;
                var Package = await PackageRepos.GetAsync(mc => mc.Id == PackageInput.Id);
                if (Package == null) {
                    throw new KeyNotFoundException();
                }
                    

                Package.Name = Package.Name;

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(Package PackageInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var PackageRepos = _unitOfWork.PackageRepository;
                PackageRepos.Add(PackageInput);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(Guid PackageId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var PackageRepository = _unitOfWork.PackageRepository;

                var Package = await PackageRepository.GetAsync(mc => mc.Id == PackageId);

                if (Package == null)
                    throw new KeyNotFoundException();

                PackageRepository.DeleteAsync(Package);

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
