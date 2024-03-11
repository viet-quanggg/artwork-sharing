using ArtworkSharing.Core.Domain.Dtos.UserDtos;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(UserToLoginDto userToLoginDTO);
        Task<UserDto> RegisterAsync(UserToRegisterDto userToRegisterDTO);
    }
}
