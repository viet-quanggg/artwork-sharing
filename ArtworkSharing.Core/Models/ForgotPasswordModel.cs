using System.ComponentModel.DataAnnotations;

namespace ArtworkSharing.Core.Models;

public class ForgotPasswordModel
{
    [Required] [EmailAddress] public string Email { get; set; }
}