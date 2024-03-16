using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Service.Services
{
    public class PaymentEventService : IPaymentEventService
    {
        private readonly IUnitOfWork _uow;

        public PaymentEventService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task AddPaymentEvent(PaymentEvent paymentEvent)
        {
            await _uow.PaymentEventRepository.AddAsync(paymentEvent);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<PaymentEvent>> GetPaymentEvents()
        {
            return await _uow.PaymentEventRepository.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task RemovePaymentEvent(PaymentEvent paymentEvent)
        {
            await _uow.PaymentEventRepository.DeleteAsync(paymentEvent);
        }
    }
}
