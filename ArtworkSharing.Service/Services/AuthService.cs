using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Service.AutoMappings;

namespace ArtworkSharing.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
      

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        public async Task<UserDto> LoginAsync(UserToLoginDto userToLoginDto)
        {
            var userMapping = AutoMapperConfiguration.Mapper.Map<User>(userToLoginDto);
            var user = await _unitOfWork.UserRepository.GetAsync(x=>x.Email.Equals(userMapping.Email));

            if (user == null)
                throw new Exception();
            if(!BCrypt.Net.BCrypt.Verify(userToLoginDto.Password, user.Password))
            {
                throw new Exception();
            }
            var userToReturn = AutoMapperConfiguration.Mapper.Map<UserDto>(userMapping);
            await _tokenService.CreateToken(userMapping);
            return userToReturn;

        }

        public async Task<UserDto> RegisterAsync(UserToRegisterDto userToRegisterDto)
        {
            var PasswordHash = BCrypt.Net.BCrypt.HashPassword(userToRegisterDto.Password);
            var user = AutoMapperConfiguration.Mapper.Map<User>(userToRegisterDto);
            user.Password = PasswordHash;
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            await _tokenService.CreateToken(user);
            return AutoMapperConfiguration.Mapper.Map<UserDto>(user);

        }
    }
}
