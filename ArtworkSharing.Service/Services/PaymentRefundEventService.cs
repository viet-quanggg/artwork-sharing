using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class PaymentRefundEventService : IPaymentRefundEventService
    {
        private readonly IUnitOfWork _uow;

        public PaymentRefundEventService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task CreatePaymentRefundEvent(PaymentRefundEvent refundEvent)
        {
            await _uow.PaymentRefundEventRepository.AddAsync(refundEvent);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<PaymentRefundEvent>> GetPaymentRefundEvents()
        {
            return await _uow.PaymentRefundEventRepository.GetAll().AsQueryable().ToListAsync();
        }

        public async Task RemovePaymentRefundEvent(PaymentRefundEvent refundEvent)
        {
            await _uow.PaymentRefundEventRepository.DeleteAsync(refundEvent);
        }
    }
}
