using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class PaymentMethodService: IPaymentMethodService
    {
        private readonly IUnitOfWork _uow;

        public PaymentMethodService(IUnitOfWork unitOfWork)
        {
            _uow=unitOfWork;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethods()
            => AutoMapperConfiguration.Mapper.Map<List<PaymentMethod>>(await _uow.PaymentMethodRepository.GetAll().AsQueryable().ToListAsync());

        public async Task<PaymentMethod> GetPaymentMethod(Guid id) 
            => AutoMapperConfiguration.Mapper.Map<PaymentMethod>(await _uow.PaymentMethodRepository.FirstOrDefaultAsync(x => x.Id == id));
    }
}
