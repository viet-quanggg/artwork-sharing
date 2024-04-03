using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class PaypalRefundEventService: IPaypalRefundEventService
    {
        private readonly IUnitOfWork _uow;

        public PaypalRefundEventService(IUnitOfWork unitOfWork)
        {
            _uow=unitOfWork;
        }

        public async Task<List<PaypalRefundEvent>> GetPaypalRefundEvents()
            => await _uow.PaypalRefundEventRepository.GetAll().AsQueryable().ToListAsync();


        public async Task RemovePaypalRefundEvent(PaypalRefundEvent paypalRefundEvent)
        {
            await _uow.PaypalRefundEventRepository.DeleteAsync(paypalRefundEvent);
        }

        public async Task AddPaypalRefundEvent(PaypalRefundEvent paypalRefundEvent)
        {
            await _uow.PaypalRefundEventRepository.AddAsync(paypalRefundEvent);
            await _uow.SaveChangesAsync();
        }
    }
}
