using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<TransactionViewModel>> GetAll()
    {
        return AutoMapperConfiguration.Mapper.Map<List<TransactionViewModel>>(await _uow.TransactionRepository.GetAll()
            .AsQueryable().ToListAsync());
    }

    public async Task<Transaction> GetOne(Guid id)
    {
        return await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id);
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
}