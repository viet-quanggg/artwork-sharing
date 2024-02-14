using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _uow;

        public TransactionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> DeleteTransaction(Guid id)
        {
            var transaction = await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            await _uow.TransactionRepository.DeleteAsync(transaction);

            var rs = await _uow.SaveChangesAsync();
            return rs > 0;
        }

        public async Task<List<TransactionViewModel>> GetAll()
            => AutoMapperConfiguration.Mapper.Map<List<TransactionViewModel>>(await (_uow.TransactionRepository.GetAll().AsQueryable()).ToListAsync());

        public async Task<TransactionViewModel> GetTransaction(Guid id)
            => AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id));

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
}
