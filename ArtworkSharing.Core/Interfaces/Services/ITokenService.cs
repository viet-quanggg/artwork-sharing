using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
