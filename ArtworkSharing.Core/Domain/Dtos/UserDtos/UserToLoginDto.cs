using System.ComponentModel.DataAnnotations;

namespace ArtworkSharing.Core.Domain.Dtos.UserDtos
{
    public class UserToLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
