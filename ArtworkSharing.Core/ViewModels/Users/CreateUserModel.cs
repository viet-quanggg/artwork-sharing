namespace ArtworkSharing.Core.ViewModels.Users;

public class CreateUserModel
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool Status { get; set; }

    public Guid RoleId { get; set; }
}