namespace ArtworkSharing.Core.Domain.Dtos.UserDtos
{
    public class UserToLoginDto
    {
        public string Email { get; set; }    
        public string Password { get; set; } = null!;
    }
}
