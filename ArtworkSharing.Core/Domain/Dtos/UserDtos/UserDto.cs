namespace ArtworkSharing.Core.Domain.Dtos.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
