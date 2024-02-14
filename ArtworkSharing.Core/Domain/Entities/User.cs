using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class User : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool Status { get; set; } 

        public Guid RoleId { get; set; }

       
        public Role Role { get; set; } = null!;
        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Followings { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<ArtworkService>? ArtworkServices { get; set; }
    }
}
