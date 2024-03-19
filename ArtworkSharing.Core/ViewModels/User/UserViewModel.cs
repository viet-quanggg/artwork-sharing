namespace ArtworkSharing.Core.ViewModels.User;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? BankAccount { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Gender { get; set; }
    public bool Status { get; set; }
    public string NormalizedUserName { get; set; }
    // public string Username { get; set; } = null!;
    // public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public Guid RoleId { get; set; }
}