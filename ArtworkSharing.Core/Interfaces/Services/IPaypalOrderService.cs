namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaypalOrderService
    {
        Task<string> GetToken();
    }
}
