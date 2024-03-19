namespace ArtworkSharing.Core.ViewModels.User;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool Status { get; set; }


    public Guid RoleId { get; set; }
}