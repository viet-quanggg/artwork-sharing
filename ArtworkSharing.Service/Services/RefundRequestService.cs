﻿using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtworkSharing.Service.Services;

public class RefundRequestService : IRefundRequestService
{
    private readonly IUnitOfWork _uow;

    public RefundRequestService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<bool> DeleteRefundRequest(Guid id)
    {
        var refundRequest = await _uow.RefundRequestRepository.FirstOrDefaultAsync(_ => _.Id == id);

        if (refundRequest == null) throw new ArgumentNullException(nameof(refundRequest));

        await _uow.RefundRequestRepository.DeleteAsync(refundRequest);

        var rs = await _uow.SaveChangesAsync();

        return rs > 0;
    }

    public async Task CreateRefundRequest(CreateRefundRequestModel crrm)
    {
        await _uow.BeginTransaction();
        var refund = AutoMapperConfiguration.Mapper.Map<RefundRequest>(crrm);
        refund.RefundRequestDate = DateTime.Now;
        var repo = _uow.RefundRequestRepository;
        await repo.AddAsync(refund);

        await _uow.CommitTransaction();
    }

    public async Task<List<RefundRequestViewModel>> GetAll()
    {
        return AutoMapperConfiguration.Mapper.Map<List<RefundRequestViewModel>>(await _uow.RefundRequestRepository
            .GetAll().AsQueryable().ToListAsync());
    }

    public async Task<RefundRequestViewModel> GetRefundRequest(Guid id)
    {
        return AutoMapperConfiguration.Mapper.Map<RefundRequestViewModel>(
            await _uow.RefundRequestRepository.FirstOrDefaultAsync(_ => _.Id == id));
    }

    public async Task<RefundRequestViewModel> UpdateRefundRequest(Guid id, UpdateRefundRequestModel urm)
    {
        var refundRequest = await _uow.RefundRequestRepository.FirstOrDefaultAsync(_ => _.Id == id);
        if (refundRequest == null) return null!;

        refundRequest.Status = urm.Status ?? refundRequest.Description;

        _uow.RefundRequestRepository.UpdateRefundRequest(refundRequest);
        await _uow.SaveChangesAsync();
        return await GetRefundRequest(id);


    }

        IEnumerable<RefundRequest> IRefundRequestService.Get(Expression<Func<RefundRequest, bool>> filter, Func<IQueryable<RefundRequest>, IOrderedQueryable<RefundRequest>> orderBy, string includeProperties, int? pageIndex, int? pageSize)
        {
            try
            {
                

                var PackageRepository = _uow.RefundRequestRepository.Get(filter, orderBy, includeProperties, pageIndex, pageSize);

                return PackageRepository;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public async Task<int> Count(Expression<Func<RefundRequest, bool>> filter = null)
        {
            IQueryable<RefundRequest> query = (IQueryable<RefundRequest>)_uow.RefundRequestRepository.GetAll();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.CountAsync();
        }

       
    }
