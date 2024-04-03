﻿using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _uow;

    public TransactionService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TransactionViewModel> AddTransaction(Transaction transaction)
    {
        await _uow.TransactionRepository.AddAsync(transaction);
        var rs = await _uow.SaveChangesAsync();
        return await GetTransaction(transaction.Id);
    }

    public async Task<bool> DeleteTransaction(Guid id)
    {
        var transaction = await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        await _uow.TransactionRepository.DeleteAsync(transaction);

        var rs = await _uow.SaveChangesAsync();
        return rs > 0;
    }

    public async Task<List<TransactionViewModel>> GetTransactions(TransactionFilterModel transactionFilter)
    {
        var trans = _uow.TransactionRepository.GetAll().AsQueryable();
        if (transactionFilter.PackageId != null! && transactionFilter.PackageId != Guid.Empty)
            trans = trans.Where(x => x.PackageId == transactionFilter.PackageId);

        if (transactionFilter.ArtworkId != null! && transactionFilter.ArtworkId != Guid.Empty)
            trans = trans.Where(x => x.ArtworkId == transactionFilter.ArtworkId);

        if (transactionFilter.ArtworkServiceId != null! && transactionFilter.ArtworkServiceId != Guid.Empty)
            trans = trans.Where(x => x.ArtworkServiceId == transactionFilter.ArtworkServiceId);

        if (transactionFilter.AudienceId != null! && transactionFilter.AudienceId != Guid.Empty)
            trans = trans.Where(x => x.AudienceId == transactionFilter.AudienceId);

        if (transactionFilter.TotalBillFrom >= 0)
            trans = trans.Where(x => x.TotalBill >= transactionFilter.TotalBillFrom);

        if (transactionFilter.TotalBillTo >= 0) trans = trans.Where(x => x.TotalBill <= transactionFilter.TotalBillTo);

        if (transactionFilter.CreatedDateFrom != null && transactionFilter.CreatedDateFrom != DateTime.MinValue)
            trans = trans.Where(x => x.CreatedDate >= transactionFilter.CreatedDateFrom);

        if (transactionFilter.CreatedDateTo != null && transactionFilter.CreatedDateTo != DateTime.MinValue)
            trans = trans.Where(x => x.CreatedDate <= transactionFilter.CreatedDateTo);

        if (transactionFilter.Status != null) trans = trans.Where(x => x.Status == transactionFilter.Status);

        if (transactionFilter.Type != null) trans = trans.Where(x => x.Type == transactionFilter.Type);

        if (transactionFilter.PageIndex >= 0 && transactionFilter.PageSize > 0)
            trans = trans.Skip((transactionFilter.PageIndex - 1) * transactionFilter.PageSize)
                .Take(transactionFilter.PageSize);
        return AutoMapperConfiguration.Mapper.Map<List<TransactionViewModel>>(await trans.ToListAsync());
    }

    public async Task<List<TransactionsViewModelUser>> GetTransactionsForUser(Guid userId) =>
        AutoMapperConfiguration.Mapper.Map<List<TransactionsViewModelUser>>(await _uow.TransactionRepository
            .Include(t => t.Audience)
                .ThenInclude(a => a.UserRoles)
            .Include(t => t.Artwork)
            .Include(t => t.PaymentMethod)
            .Where(t => t.AudienceId == userId)
            .AsQueryable()
            .ToListAsync());


    public async Task<TransactionViewModel> CreateTransactionForArtworkRequestDeposit(Guid artworkrequestId, Guid audienceId, Guid paymentMethodId)
    {
        try
        {
            await _uow.BeginTransaction();
            var artworkRequestRepo = _uow.ArtworkServiceRepository;
            var transactionRepo = _uow.TransactionRepository;
            var paymentMethodRepo = _uow.PaymentMethodRepository;

            var artworkService = await artworkRequestRepo.FirstOrDefaultAsync(a => a.Id == artworkrequestId);
            var paymentMethod = await paymentMethodRepo.FirstOrDefaultAsync(a => a.Id == paymentMethodId);

            Transaction transaction = new Transaction();
            transaction.Id = Guid.NewGuid();
            transaction.ArtworkServiceId = artworkrequestId;
            transaction.AudienceId = audienceId;
            transaction.CreatedDate = DateTime.Now;
            transaction.TotalBill = artworkService.RequestedDeposit;
            transaction.Type = TransactionType.ArtworkService;
            transaction.Status = TransactionStatus.Pending;
            transaction.ArtworkService = artworkService;
            transaction.PaymentMethod = paymentMethod;

            await transactionRepo.AddAsync(transaction);
            await _uow.SaveChangesAsync();
            await _uow.CommitTransaction();

            return AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(transaction);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<List<TransactionViewModel>> GetAll()
    {
        return AutoMapperConfiguration.Mapper.Map<List<TransactionViewModel>>(await _uow.TransactionRepository.GetAll()
            .AsQueryable().ToListAsync());
    }

    public async Task<Transaction> GetOne(Guid id)
    {
        return await _uow.TransactionRepository.Include(x => x.Audience).Include(x => x.Artwork).Include(x => x.ArtworkService).Include(x => x.Package).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TransactionViewModel> GetTransaction(Guid id)
    {
        return AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(
            await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id));
    }

    public async Task<TransactionViewModel> UpdateTransaction(Guid id, UpdateTransactionModel utm)
    {
        var transaction = await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        transaction.Status = utm.Status;
        transaction.PackageId = utm.PackageId ?? transaction.PackageId;
        transaction.ArtworkId = utm.ArtworkId ?? transaction.ArtworkId;
        transaction.ArtworkServiceId = utm.ArtworkServiceId ?? transaction.ArtworkServiceId;

        // Add whatever you need

        _uow.TransactionRepository.UpdateTransaction(transaction);
        await _uow.SaveChangesAsync();
        return await GetTransaction(id);
    }

    public async Task<TransactionViewModel> CreateTransactionArtwork(TransactionCreateModel transactionCreateModel)
    {
        Transaction transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            ArtworkId = transactionCreateModel.ArtworkId,
            TotalBill = transactionCreateModel.TotalBill,
            CreatedDate = DateTime.Now,
            PaymentMethodId = transactionCreateModel.PaymentMethodId,
            Type = TransactionType.Artwork,
            Status = TransactionStatus.Pending
        };
        await _uow.TransactionRepository.AddAsync(transaction);
        await _uow.SaveChangesAsync();
        return AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(await GetOne(transaction.Id));
    }

    IEnumerable<Transaction> ITransactionService.Get(Expression<Func<Transaction, bool>> filter, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy, string includeProperties, int? pageIndex, int? pageSize)
    {
        try
        {


            var PackageRepository = _uow.TransactionRepository.Get(filter, orderBy, includeProperties, pageIndex, pageSize);

            return PackageRepository;
        }
        catch (Exception e)
        {

            return null;
        }
    }

    public async Task<int> Count(Expression<Func<Transaction, bool>> filter = null)
    {
        IQueryable<Transaction> query = (IQueryable<Transaction>)_uow.TransactionRepository.GetAll();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.CountAsync();
    }
    public async Task<List<TransactionsViewModelUser>> GetAudience() =>
      AutoMapperConfiguration.Mapper.Map<List<TransactionsViewModelUser>>(await _uow.TransactionRepository
          .Include(t => t.Audience).AsQueryable()
            .ToListAsync());
}
