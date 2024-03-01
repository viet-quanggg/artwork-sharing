using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class User : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;       
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool Status { get; set; }

        public int RoleId { get; set; } = 3;


        public Role Role { get; set; } = new Role { Name = "User" };
        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Followings { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<ArtworkService>? ArtworkServices { get; set; }
    }
}
