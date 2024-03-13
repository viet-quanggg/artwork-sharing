using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.VNPAYS;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IPaymentService
{
    string GetUrlFromTransaction(Transaction trans);
    Task<VNPayViewModel> HandleQuery(string query);
}