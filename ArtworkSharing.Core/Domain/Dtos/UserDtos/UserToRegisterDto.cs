namespace ArtworkSharing.Core.Domain.Dtos.UserDtos
{
    public class UserToRegisterDto
    {
        
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;      
        public string Username { get; set; } = null!;      
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
