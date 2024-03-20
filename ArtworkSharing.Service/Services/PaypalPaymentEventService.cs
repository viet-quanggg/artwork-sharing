using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class PaypalPaymentEventService : IPaypalPaymentEventService
    {
        private readonly IUnitOfWork _uow;

        public PaypalPaymentEventService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<List<PaypalPaymentEvent>> GetPaypalPaymentEvents()
            => await _uow.PaypalPaymentEventRepository.GetAll().AsQueryable().ToListAsync();

        public async Task RemovePaypalPaymentEvent(PaypalPaymentEvent paypalPaymentEvent)
        {
            await _uow.PaypalPaymentEventRepository.DeleteAsync(paypalPaymentEvent);
        }

        public async Task AddPaypalPaymentEvent(PaypalPaymentEvent paypalPaymentEvent)
        {
            await _uow.PaypalPaymentEventRepository.AddAsync(paypalPaymentEvent);
            await _uow.SaveChangesAsync();
        }
    }
}
