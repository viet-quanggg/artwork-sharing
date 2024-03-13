using Microsoft.AspNetCore.Identity;

namespace ArtworkSharing.Core.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = null!;
    public string? BankAccount { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Gender { get; set; }
    public bool Status { get; set; }

    public ICollection<Follow>? Followers { get; set; }
    public ICollection<Follow>? Followings { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<ArtworkService>? ArtworkServices { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}